src = "https://cdn.jsdelivr.net/npm/jwt-decode/build/jwt-decode.min.js"
function getRoleFromToken() {
    const token = getCookie("jwtToken");
    if (token) {
        const decoded = jwt_decode(token);
        return decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    }
    return null;
}
let idUsuario;
function obtenerIdUsuario() {
    const token = getCookie("jwtToken");
    if (token) {
        const decoded = jwt_decode(token);
        idUsuario = decoded["id"];
    }
}
function getUserIdFromToken() {
    const token = getCookie("jwtToken");
    if (token) {
        const decoded = jwt_decode(token);
        return decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    }
    return null;
}
document.addEventListener("DOMContentLoaded", function () {
    const userRole = getRoleFromToken();

    if (userRole === "Gerente") {
        document.getElementById("adminOptions").style.display = "block";
    } else {
        document.getElementById("adminOptions").style.display = "none";
    }
});
function setCookie(name, value, hours) {
    const d = new Date();
    d.setTime(d.getTime() + (hours * 60 * 60 * 1000));
    const expires = "expires=" + d.toUTCString();
    document.cookie = name + "=" + value + ";" + expires + ";path=/";
}


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
            console.log("Respuesta Login");

            if (data && data.response && data.response.token) {

                setCookie("jwtToken", data.response.token, 2);

                document.getElementById("remoteResponse").innerText = "Login exitoso.";
                window.location.href = "/Home/LoginExitoso";
            } else {
                document.getElementById("remoteResponse").innerText = "Error en el login.";
            }
        })
        .catch(error => {
            document.getElementById("remoteResponse").innerText = 'Error de CORS: ' + error;
        });
}


function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}
function listarUsuarios() {
    const queryURL = "http://localhost:5005/api/Usuario/listarUsuarios";
    const token = getCookie("jwtToken");
    if (!token) {
        window.location.href = "/Home/Login";
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
            if (!response.ok) {
                throw new Error('Error en la respuesta de la API');
            }
            return response.json();
        })
        .then(data => {
            console.log("Usuarios obtenidos:", data);

            let usuariosTableBody = document.getElementById("usuariosTableBody");
            usuariosTableBody.innerHTML = ""; 

            
            data.forEach(usuario => {
                usuariosTableBody.innerHTML += `
            <tr>
                <td>${usuario.nombre}</td>
                <td>${usuario.apellido}</td>
                <td>${usuario.correo}</td>
                <td>
                    <a onclick="confirmarEliminacionUsuario('${usuario.correo}')" class="d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm">
                        <i class="fas fa-trash-alt"></i> Eliminar
                    </a>
                </td>
            </tr>
        `;
            });
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
function confirmarEliminacionUsuario(correo) {
    const confirmacion = confirm(`¿Estás seguro que deseas eliminar al usuario con correo: ${correo}?`);

    if (confirmacion) {

        eliminarUsuario(correo);
    }
}
function eliminarUsuario(correo) {
    const queryURL = `http://localhost:5005/api/Usuario/EliminarUsuario/${correo}`;
    const token = getCookie("jwtToken");
    if (!token) {
        window.location.href = "/Home/Login";
    }
    fetch(queryURL, {
        method: 'DELETE',
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al eliminar el usuario.');
            }
            return response.json();
        })
        .then(data => {
            alert(data.mensaje);
            listarUsuarios();
        })
        .catch(error => {
            console.error('Error:', error);
            alert("No se pudo eliminar el usuario.");
        });
}
function crearUsuario() {
    const queryURL = "http://localhost:5005/api/Usuario/CrearUsuario";
    const token = getCookie("jwtToken");
    if (!token) {
        window.location.href = "/Home/Login";
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
    if (!token) {
        window.location.href = "/Home/Login";
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
            if (!response.ok) {
                throw new Error('Error al cerrar sesión');
            }
            return response.json();
        })
        .then(data => {

            setCookie("jwtToken", "", -1);
            alert(data.mensaje);
            window.location.href = "/Home/Login";
        })
        .catch(error => {
            alert("Error cerrando sesión: " + error.message);
        });
}
document.addEventListener("DOMContentLoaded", function () {
    listarSemillas();
    listarUsuarios();
    listarPedidos()
});

function crearSemilla() {
    const queryURL = "http://localhost:5005/api/Semilla/CrearSemilla";
    const nuevaSemilla = {
        "nombre": document.getElementById("nombre").value,
        "codigo": document.getElementById("codigo").value,
        "descripcion": document.getElementById("descripcion").value,
        "cantidad": parseInt(document.getElementById("cantidad").value),
        "idCategoria": parseInt(document.getElementById("idCategoria").value),
        "ubicacion": document.getElementById("ubicacion").value
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
                listarSemillas();
                alert("Semilla registrada exitosamente.");
                window.location.href = "/Home/VistaInventario";
            } else {
                alert("Error al registrar la semilla.");
            }
        })
        .catch(error => {
            console.error("Error:", error);
        });
}

function listarSemillas() {
    const queryURL = "http://localhost:5005/api/Semilla";
    const token = getCookie("jwtToken");

    fetch(queryURL, {
        headers: {
            'Authorization': 'Bearer ' + token,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            const semillasTableBody = document.getElementById("semillasTableBody");
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
                    <a onclick="window.location.href='/Home/EditarCantidad?id=${semilla.id}'" class="d-none d-sm-inline-block btn btn-sm btn-success shadow-sm">
                        <i class="fas fa-pen"></i> Actualizar Cantidad
                    </a>
                    <a onclick="window.location.href='/Home/Traslados?id=${semilla.id}'" class="d-none d-sm-inline-block btn btn-sm btn-dark shadow-sm">
                        <i class="fas fa-pen"></i> Hacer Traslado
                    </a>
                    <a onclick="confirmarEliminacionSemilla('${semilla.id}')" class="d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm">
                        <i class="fas fa-trash-alt"></i> Eliminar
                    </a>
                    </td>
                    
                </tr>
            `;
                semillasTableBody.innerHTML += row;
            });
        })
        .catch(error => {
            console.error("Error al listar las semillas:", error);
        });
}
function actualizarCantidad() {
    const id = new URLSearchParams(window.location.search).get("id");
    const nuevaCantidad = document.getElementById("nuevaCantidad").value;

    if (!id || !nuevaCantidad) {
        alert("ID de la semilla o cantidad no válida.");
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
    const id = new URLSearchParams(window.location.search).get("id");
    const trasladoDto = {
        nuevaUbicacion: document.getElementById("nuevaUbicacion").value,
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

    if (!numeroPedido || isNaN(nuevoEstadoId)) {
        alert("Número de pedido o estado no válido");
        return;
    }

    const dto = {
        NumeroPedido: numeroPedido,
        NuevoEstadoId: nuevoEstadoId
    };

    // Hacer la solicitud con fetch y manejar las promesas con .then() y .catch()
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

function agregarSemilla() {
    const semillasContainer = document.getElementById("semillasContainer");
    const semillaHTML = `<div class="semilla">
                            <div class="form-group row">
                                <div class="col-sm-6 mb-3 mb-sm-0">
                                    <input class="form-control form-control-user" placeholder="Id de Semilla" type="number" class="semillaId" required>
                                </div>
                                <div class="col-sm-6">
                                    <input class="form-control form-control-user" placeholder="Cantidad" type="number" class="cantidad" required>
                                </div>
                                
                    <br><br>
                            </div>
                            <a onclick="eliminarSemilla(this)" class="d-none d-sm-inline-block btn btn-sm btn-danger shadow-sm">
                    <i class="fas fa-pen"></i> Eliminar
                    </a>
                    <br><br>
                </div>
            `;
    semillasContainer.insertAdjacentHTML("beforeend", semillaHTML);
}

function eliminarSemilla(button) {
    button.parentNode.remove();
}

function registrarPedido(event) {
    event.preventDefault();
    const numeroPedido = document.getElementById("numeroPedido").value;
    const estadoId = parseInt(document.getElementById("estadoId").value);
    const notasEnvio = document.getElementById("notasEnvio").value;

    const detalles = [];
    const semillas = document.querySelectorAll(".semilla");
    semillas.forEach(semilla => {
        const semillaId = parseInt(semilla.querySelector(".semillaId").value);
        const cantidad = parseInt(semilla.querySelector(".cantidad").value);
        detalles.push({ semillaId, cantidad });
    });

    const pedido = {
        numeroPedido,
        estadoId,
        notasEnvio,
        detalles
    };

    fetch("http://localhost:5005/api/pedido/crear", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(pedido)
    })
        .then(response => {
            if (response.ok) {
                alert("Pedido registrado exitosamente.");
                window.location.href = "/Home/VistaPedidos";
            } else {
                throw new Error("Error al registrar el pedido.");
            }
        })
        .catch(error => alert("No se pudo registrar el pedido: " + error));
}
