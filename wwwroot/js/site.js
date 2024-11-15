src = "https://cdn.jsdelivr.net/npm/jwt-decode/build/jwt-decode.min.js";

function getRoleFromToken() {
    const token = getCookie("jwtToken");
    if (!token) return null;

    try {
        const decoded = jwt_decode(token);
        console.log("Contenido del token decodificado:", decoded);
        const currentTime = Date.now() / 1000;
        if (decoded.exp && decoded.exp < currentTime) {
            console.warn("Token expirado.");
            setCookie("jwtToken", "", -1);
            return null;
        }
        return decoded["role"] || null;
    } catch (error) {
        console.error("Error al decodificar el token:", error);
        setCookie("jwtToken", "", -1);
        return null;
    }
}
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    return parts.length === 2 ? parts.pop().split(';').shift() : null;
}

function setCookie(name, value, hours) {
    const d = new Date();
    d.setTime(d.getTime() + (hours * 60 * 60 * 1000));
    const expires = "expires=" + d.toUTCString();
    document.cookie = `${name}=${value}; ${expires}; path=/; Secure`;
}
document.addEventListener("DOMContentLoaded", function () {
    listarSemillas();
    listarUsuarios();
    listarPedidos()
});
document.addEventListener("DOMContentLoaded", function () {
    const token = getCookie("jwtToken");
    const userRole = token ? getRoleFromToken() : null;
    console.log("Rol del usuario:", userRole);

    
    const isManager = userRole === "2";
    const isAdmin = userRole === "1";

    const unprotectedRoutes = ["/Home/Login", "/Home/RecuperarContra", "/Home/LoginExitoso"];

    
    if (!userRole && !unprotectedRoutes.includes(window.location.pathname)) {
        console.warn("No se ha encontrado rol o el token es inválido. Redirigiendo al login.");
        window.location.href = "/Home/Login";
        return;
    }

    
    if (isManager) {
        console.log("Acceso completo habilitado para el Gerente.");
        document.querySelectorAll(".nav-link").forEach(link => link.style.display = "block");
    } else if (isAdmin) {
        console.log("Acceso restringido para el Administrador.");
        document.querySelectorAll(".nav-link").forEach(link => {
            if (link.classList.contains("admin-only")) {
                link.style.display = "block";
            } else {
                link.style.display = "none";
            }
        });
    }else if (!unprotectedRoutes.includes(window.location.pathname)) {
    console.warn("Redirigiendo al login.");
    window.location.href = "/Home/Login";
    }
    llenarSelectorLetras();
    llenarSelectorAlturas();
    
});

function login() {
    const queryURL = "http://localhost:5005/api/Usuario/Login";
    const item = {
        "correo": document.getElementById("Correo").value,
        "contrasena": document.getElementById("Contrasena").value
    };

    fetch(queryURL, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(data => {
            console.log("Respuesta de login:", data);

            if (data && data.response && data.response.token) {
                setCookie("jwtToken", data.response.token, 2);
                document.getElementById("remoteResponse").innerText = "Login exitoso.";
                window.location.href = "/Home/LoginExitoso";
            } else {
                document.getElementById("remoteResponse").innerText = "Error en el login.";
            }
        })
        .catch(error => {
            document.getElementById("remoteResponse").innerText = 'Error de CORS o red: ' + error;
        });
}

function listarUsuarios() {
    const queryURL = "http://localhost:5005/api/Usuario/listarUsuarios";
    const token = getCookie("jwtToken");

    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }

    fetch(queryURL, {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (response.status === 401) {
                sessionStorage.setItem("redirected", "true");
                window.location.href = "/Home/Login";
                return;
            }
            return response.json();
        })
        .then(data => {
            const usuariosTableBody = document.getElementById("usuariosTableBody");
            if (!usuariosTableBody) {
                console.warn("Elemento 'usuariosTableBody' no encontrado en esta vista.");
                return;
            }

            usuariosTableBody.innerHTML = "";
            data.forEach(usuario => {
                usuariosTableBody.innerHTML += `
                <tr>
                    <td>${usuario.nombre}</td>
                    <td>${usuario.apellido}</td>
                    <td>${usuario.correo}</td>
                    <td>${usuario.isActive ? 'Activo' : 'Inactivo'}</td>
                    <td>
                        ${usuario.isActive ?
                        `<a onclick="confirmarInhabilitacionUsuario('${usuario.correo}')" class="btn btn-danger btn-sm">Inhabilitar</a>` :
                        `<a onclick="activarUsuario('${usuario.correo}')" class="btn btn-success btn-sm">Activar</a>`
                    }
                    </td>
                </tr>
            `;
            });
        })
        .catch(error => console.error('Error al listar usuarios:', error));
}

function activarUsuario(correo) {
    const queryURL = `http://localhost:5005/api/Usuario/ActivarUsuario/${correo}`;
    const token = getCookie("jwtToken");

    fetch(queryURL, {
        method: 'PATCH',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (!response.ok) throw new Error('Error al activar el usuario.');
            return response.json();
        })
        .then(data => {
            alert(data.mensaje || "Usuario activado exitosamente.");
            listarUsuarios();
        })
        .catch(error => console.error("Error al activar usuario:", error));
}

function confirmarInhabilitacionUsuario(correo) {
    const confirmacion = confirm(`¿Estás seguro que deseas inhabilitar al usuario con correo: ${correo}?`);
    if (confirmacion) {
        inhabilitarUsuario(correo);
    }
}

function inhabilitarUsuario(correo) {
    const queryURL = `http://localhost:5005/api/Usuario/InhabilitarUsuario/${correo}`;
    const token = getCookie("jwtToken");

    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }

    fetch(queryURL, {
        method: 'PATCH', // Método PATCH para inhabilitar
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al inhabilitar el usuario.');
            }
            return response.json();
        })
        .then(data => {
            alert(data.mensaje || "Usuario inhabilitado exitosamente.");
            listarUsuarios();
        })
        .catch(error => {
            console.error('Error:', error);
            alert("No se pudo inhabilitar el usuario.");
        });
}

function crearUsuario() {
    const queryURL = "http://localhost:5005/api/Usuario/CrearUsuario";
    const token = getCookie("jwtToken");
    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }
    const usuario = {
        "nombre": document.getElementById("nombre").value,
        "apellido": document.getElementById("apellido").value,
        "correo": document.getElementById("correo").value,
        "contrasena": document.getElementById("contrasena").value,
        "idRol": document.getElementById("idrol").value,
        "numDocumento": document.getElementById("numerodocumento").value
    };

    fetch(queryURL, {
        method: 'POST',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(usuario)
    })
        .then(response => {
            if (!response.ok) {

                throw new Error('Error al crear el usuario');
            }
            return response.json();
        })
        .then(data => {

            alert("Usuario creado exitosamente.");
            console.log("Usuario creado:", data);
            window.location.href = "/Home/ListarUsuarios";
        })
        .catch(error => {

            alert("No se pudo crear el usuario. Error: " + error.message);
            console.error('Error:', error);
        });
}
function RecuperarContrasena() {
    const queryURL = "http://localhost:5005/api/Usuario/RecuperarContrasena";

    const usuario = {
        "correo": document.getElementById("correo").value
    };

    fetch(queryURL, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(usuario)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al enviar el correo');
            }
            return response.json();
        })
        .then(data => {
            alert("Correo enviado exitosamente.");
            window.location.href = "/Home/ConfirmarTokenContrasena";  
        })
        .catch(error => {
            alert("Error al enviar el correo: " + error.message);
            console.error('Error:', error);
        });
}
function ConfirmarTokenContrasena() {
    const queryURL = "http://localhost:5005/api/Usuario/ConfirmarTokenContrasena";

    const usuario = {
        "token": document.getElementById("token").value,
        "nuevaContrasena": document.getElementById("nuevaContrasena").value
    };

    fetch(queryURL, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(usuario)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al confirmar la contraseña');
            }
            return response.json();
        })
        .then(data => {
            alert("Contraseña cambiada exitosamente.");
            window.location.href = "/Home/Login";
        })
        .catch(error => {
            alert("Error al cambiar la contraseña: " + error.message);
            console.error('Error:', error);
        });
}
function cerrarSesion() {
    const token = getCookie("jwtToken");

    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }

    fetch("http://localhost:5005/api/Usuario/CerrarSesion", {
        method: 'POST',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (response.ok) {
                setCookie("jwtToken", "", -1); 
                alert("Sesión cerrada correctamente.");
                window.location.href = "/Home/Login";
            } else {
                throw new Error('Error al cerrar sesión');
            }
        })
        .catch(error => {
            console.error("Error cerrando sesión:", error);
            alert("No se pudo cerrar la sesión.");
        });
}


function crearSemilla() {
    const queryURL = "http://localhost:5005/api/Semilla/CrearSemilla";
    const token = getCookie("jwtToken");
    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }
    const ubicacion = `${document.getElementById("ubicacionLetra").value}-${document.getElementById("ubicacionNumero1").value}-${document.getElementById("ubicacionNumero2").value}`;
    const nuevaSemilla = {
        nombre: document.getElementById("nombre").value,
        codigo: document.getElementById("codigo").value,
        descripcion: document.getElementById("descripcion").value,
        cantidad: parseInt(document.getElementById("cantidad").value),
        idCategoria: parseInt(document.getElementById("idCategoria").value),
        ubicacion: ubicacion
    };

    fetch(queryURL, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(nuevaSemilla)
    })
        .then(response => {
            if (response.ok) {
                alert("Semilla registrada exitosamente.");
                window.location.href = "/Home/VistaInventario";
            } else {
                alert("Error al registrar la semilla. Verifique si la ubicación ya está ocupada.");
            }
        })
        .catch(error => {
            console.error("Error:", error);
        });
}
function llenarSelectorLetras() {
    const ubicacionLetra = document.getElementById("ubicacionLetra");
    ubicacionLetra.innerHTML = '<option value="" disabled selected>Letra (A-Z)</option>';
    for (let i = 0; i < 26; i++) {
        const option = document.createElement("option");
        option.value = String.fromCharCode(65 + i);
        option.textContent = String.fromCharCode(65 + i);
        ubicacionLetra.appendChild(option);
    }
}

function llenarSelectorAlturas() {
    const ubicacionNumero2 = document.getElementById("ubicacionNumero2");
    ubicacionNumero2.innerHTML = '<option value="" disabled selected>Altura (10-70)</option>';
    for (let i = 1; i <= 7; i++) {
        const option = document.createElement("option");
        option.value = i * 10;
        option.textContent = i * 10;
        ubicacionNumero2.appendChild(option);
    }
}
function listarSemillas() {
    const queryURL = "http://localhost:5005/api/Semilla";
    const token = getCookie("jwtToken");
    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }

    fetch(queryURL, {
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            // Llena la tabla de semillas
            const semillasTableBody = document.getElementById("semillasTableBody");
            if (semillasTableBody) {
                semillasTableBody.innerHTML = "";
                data.forEach(semilla => {
                    const row = `
                    <tr>
                        <td>${semilla.id}</td>
                        <td>${semilla.nombre}</td>
                        <td>${semilla.codigo}</td>
                        <td>${semilla.descripcion}</td>
                        <td>${semilla.cantidad}</td>
                        <td>${semilla.idCategoria}</td>
                        <td>${semilla.ubicacion}</td>
                        <td>
                            <a onclick="window.location.href='/Home/EditarCantidad?id=${semilla.id}'" class="btn btn-success btn-sm">Actualizar Cantidad</a>
                            <a onclick="window.location.href='/Home/Traslados?id=${semilla.id}'" class="btn btn-dark btn-sm">Hacer Traslado</a>
                            <a onclick="confirmarEliminacionSemilla('${semilla.id}')" class="btn btn-danger btn-sm">Eliminar</a>
                        </td>
                    </tr>
                    `;
                    semillasTableBody.innerHTML += row;
                });
            }

            // Llena el select de semillas para los pedidos
            const semillaSelect = document.getElementById("semillaSelect");
            if (semillaSelect) {
                semillaSelect.innerHTML = `<option value="" disabled selected>Selecciona una semilla</option>`;
                data.forEach(semilla => {
                    const option = document.createElement("option");
                    option.value = semilla.id;
                    option.text = semilla.nombre;
                    semillaSelect.appendChild(option);
                });
            }
        })
        .catch(error => {
            console.error("Error al listar las semillas:", error);
        });
}



//function listarSemillas() {
//    const queryURL = "http://localhost:5005/api/Semilla";
//    const token = getCookie("jwtToken");
//    if (!token || !getRoleFromToken()) {
//        if (!sessionStorage.getItem("redirected")) {
//            sessionStorage.setItem("redirected", "true");
//            window.location.href = "/Home/Login";
//        }
//        return;
//    }

//    fetch(queryURL, {
//        headers: {
//            'Authorization': 'Bearer ' + token,
//            'Accept': 'application/json',
//            'Content-Type': 'application/json'
//        }
//    })
//        .then(response => response.json())
//        .then(data => {
//            const semillasTableBody = document.getElementById("semillasTableBody");
//            semillasTableBody.innerHTML = "";

//            data.forEach(semilla => {
//                const row = `
//                <tr>
//                    <td>${semilla.id}</td>
//                    <td>${semilla.nombre}</td>
//                    <td>${semilla.codigo}</td>
//                    <td>${semilla.descripcion}</td>
//                    <td>${semilla.cantidad}</td>
//                    <td>${semilla.idCategoria}</td>
//                    <td>${semilla.ubicacion}</td>
//                    <td>
//                    <a onclick="window.location.href='/Home/EditarCantidad?id=${semilla.id}'" class="d-none d-sm-inline-block btn btn-sm btn-success shadow-sm">
//                        <i class="fas fa-pen"></i> Actualizar Cantidad
//                    </a>
//                    <a onclick="window.location.href='/Home/Traslados?id=${semilla.id}'" class="d-none d-sm-inline-block btn btn-sm btn-dark shadow-sm">
//                        <i class="fas fa-pen"></i> Hacer Traslado
//                    </a>
//                    <a onclick="confirmarEliminacionSemilla('${semilla.id}')" class="d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm">
//                        <i class="fas fa-trash-alt"></i> Eliminar
//                    </a>
//                    </td>
                    
//                </tr>
//            `;
//                semillasTableBody.innerHTML += row;
//            });
//        })
//        .catch(error => {
//            console.error("Error al listar las semillas:", error);
//        });
//}
function actualizarCantidad() {
    const id = new URLSearchParams(window.location.search).get("id");

    const nuevaCantidad = document.getElementById("nuevaCantidad").value;
    const token = getCookie("jwtToken");
    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }
    const queryURL = `http://localhost:5005/api/Semilla/cantidad${id}`;

    fetch(queryURL, {
        method: 'PATCH',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(parseInt(nuevaCantidad))
    })
        .then(response => {
            if (response.ok) {
                alert("Cantidad actualizada exitosamente.");
                window.location.href = "/Home/VistaInventario";
                listarSemillas(); 
            } else {
                throw new Error("Error al actualizar la cantidad.");
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("Error al actualizar la cantidad.");
        });
}

function trasladarSemilla() {
    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }
    const id = new URLSearchParams(window.location.search).get("id");
    const nuevaUbicacion = `${document.getElementById("ubicacionLetra").value}-${document.getElementById("ubicacionNumero1").value}-${document.getElementById("ubicacionNumero2").value}`;
    const trasladoDto = {
        nuevaUbicacion: nuevaUbicacion,
        idUsuario: parseInt(document.getElementById("idUsuario").value)
    };
    const queryURL = `http://localhost:5005/api/Semilla/traslado${id}`;
    fetch(queryURL, {
        method: 'POST',
        headers: {
            
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(trasladoDto)
    })
        .then(response => {
            if (response.ok) {
                alert("Traslado de semilla exitoso.");
                window.location.href = "/Home/VistaInventario";
                listarSemillas(); 
            } else {
                alert("Error al trasladar la semilla.");
            }
        })
        .catch(error => {
            console.error("Error:", error);
            alert("No se pudo realizar el traslado de la semilla.");
        });
}
function confirmarEliminacionSemilla(id) {
    const confirmacion = confirm(`¿Estás seguro que deseas eliminar la semilla con ID: ${id}?`);

    if (confirmacion) {
        eliminarSemilla(id);
    }
}

function eliminarSemilla(id) {
    const queryURL = `http://localhost:5005/api/Semilla/Eliminar${id}`;
    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }
    fetch(queryURL, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al eliminar la semilla.');
            }
            return response.json();
        })
        .then(data => {
            alert(data.mensaje || "Semilla eliminada exitosamente.");
            listarSemillas(); 
        })
        .catch(error => {
            console.error('Error:', error);
            alert("No se pudo eliminar la semilla.");
        });
}
function listarPedidos() {
    const queryURL = 'http://localhost:5005/api/Pedido/listar';
    const token = getCookie("jwtToken");
    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }
    fetch(queryURL)
        .then(response => response.json())
        .then(data => {
            const pedidosTableBody = document.getElementById("pedidosTableBody");
            pedidosTableBody.innerHTML = "";

            data.forEach(pedido => {
                const row = `
                            <tr>
                                <td>${pedido.numeroPedido}</td>
                                <td>${pedido.estadoId}</td>
                                <td>${pedido.notasEnvio}</td>
                                <td>
                    <a onclick="window.location.href='/Home/CambiarEstado?id=${pedido.numeroPedido}'" class="d-none d-sm-inline-block btn btn-sm btn-success shadow-sm">
                        <i class="fas fa-pen"></i> Actualizar Estado
                    </a>
                    
                    </td>
                                
                            </tr>
                        `;
                pedidosTableBody.innerHTML += row;
            });
        })
        .catch(error => console.error("Error al listar los pedidos:", error));
}

function cambiarEstadoPedido() {
    const numeroPedido = new URLSearchParams(window.location.search).get("id");
    const nuevoEstadoId = parseInt(document.getElementById("nuevaEstadoId").value);
    const token = getCookie("jwtToken");
    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }

    if (!numeroPedido || isNaN(nuevoEstadoId)) {
        alert("Número de pedido o estado no válido");
        return;
    }

    const dto = {
        NumeroPedido: numeroPedido,
        NuevoEstadoId: nuevoEstadoId
    };

    
    fetch("http://localhost:5005/api/pedido/cambiar-estado", {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(dto)
    })
        .then(response => {
            if (response.ok) {
                alert("Estado actualizado con éxito");
                window.location.href = "/Home/ListarPedidos";
            } else {
                return response.text().then(errorText => {
                    alert("Error al actualizar el estado: " + errorText);
                });
            }
        })
        .catch(error => {
            console.error("Error en la solicitud:", error);
            alert("Hubo un error al actualizar el estado del pedido.");
        });
}

//function agregarSemilla() {
//    const semillasContainer = document.getElementById("semillasContainer");
//    const semillaHTML = `
//        <div class="semilla">
//            <div class="form-group row">
//                <div class="col-sm-6 mb-3 mb-sm-0">
//                    <select id="semillaSelect" class="form-control" required>
//                        <option value="" disabled selected>Selecciona una semilla</option>
//                    </select>
//                </div>
//                <div class="col-sm-6">
//                    <input class="form-control form-control-user" id="cantidad" placeholder="Cantidad" type="number" required>
//                </div>
//            </div>
//            <a onclick="eliminarSemilla(this)" class="d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm">
//                <i class="fas fa-pen"></i> Eliminar
//            </a>
//            <br><br>
//        </div>
//    `;
//    semillasContainer.insertAdjacentHTML("beforeend", semillaHTML);
//}
function agregarSemilla() {
    const semillasContainer = document.getElementById("semillasContainer");

    const semillaHTML = `
        <div class="semilla">
            <div class="form-group row">
                <div class="col-sm-6 mb-3 mb-sm-0">
                    <select class="form-control semillaSelect" required>
                        <option value="" disabled selected>Selecciona una semilla</option>
                    </select>
                </div>
                <div class="col-sm-6">
                    <input class="form-control form-control-user" placeholder="Cantidad" type="number" required>
                </div>
            </div>
            <a onclick="eliminarSemilla(this)" class="d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm">
                <i class="fas fa-pen"></i> Eliminar
            </a>
            <br><br>
        </div>
    `;

    // Insert the HTML
    semillasContainer.insertAdjacentHTML("beforeend", semillaHTML);

    // Get the newly added select element and fill it with options
    const newSelect = semillasContainer.querySelector(".semilla:last-child .semillaSelect");
    llenarSelectSemillas(newSelect);
}

function eliminarSemilla(button) {
    button.parentNode.remove();
}
function llenarSelectSemillas(selectElement) {
    const queryURL = "http://localhost:5005/api/Semilla";
    const token = getCookie("jwtToken");

    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }

    fetch(queryURL, {
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            selectElement.innerHTML = "<option value=''>Selecciona una semilla</option>";

            data.forEach(semilla => {
                const option = document.createElement("option");
                option.value = semilla.id;
                option.textContent = semilla.nombre;
                selectElement.appendChild(option);
            });
        })
        .catch(error => {
            console.error("Error al cargar las semillas en el select:", error);
        });
}

function registrarPedido() {
    const queryURL = 'http://localhost:5005/api/pedido/crear';
    const token = getCookie("jwtToken");
    if (!token || !getRoleFromToken()) {
        if (!sessionStorage.getItem("redirected")) {
            sessionStorage.setItem("redirected", "true");
            window.location.href = "/Home/Login";
        }
        return;
    }

    const numeroPedido = document.getElementById("numeroPedido").value;
    const estadoId = parseInt(document.getElementById("estadoId").value);
    const notasEnvio = document.getElementById("notasEnvio").value;
    const detalles = [];
    const semillas = document.querySelectorAll(".semilla");

    semillas.forEach(semilla => {
        const semillaIdInput = semilla.querySelector("#semillaId");
        const cantidadInput = semilla.querySelector("#cantidad");

        if (semillaIdInput && cantidadInput) {
            const semillaSelect = document.getElementById("semillaSelect");
            const semillaId = parseInt(semillaSelect.value);
            const cantidad = parseInt(cantidadInput.value);
            detalles.push({ semillaId, cantidad });
        } else {
            console.error("No se encontró un campo semillaId o cantidad en el elemento semilla.");
        }
    });

    const pedido = {
        numeroPedido,
        estadoId,
        notasEnvio,
        detalles
    };

    fetch(queryURL, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            },
        body: JSON.stringify(pedido)
    })
        .then(response => {
            if (response.ok) {
                alert("Pedido registrado exitosamente.");
                window.location.href = "/Home/ListarPedidos";
            } else {
                return response.json().then(errorData => {
                    console.error("Error en la respuesta del servidor:", errorData);
                    alert("Error al registrar el pedido: " + (errorData.message || "Error desconocido en el servidor"));
                });
            }
        })
        .catch(error => {
            console.error("Error en la solicitud de fetch:", error);
            alert("No se pudo registrar el pedido: " + error.message);
        });
}