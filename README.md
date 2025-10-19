# 🌐 Sistema de Gestión Empresarial Interna — Universal de Cauchos Hurtado S.A.S.

**Proyecto MVP — Módulos de Compras y Logística**  
Desarrollado con **ASP.NET MVC (.NET 8)** y **SQL Server**

---

## 📘 Introducción

Este proyecto corresponde al desarrollo del **Producto Mínimo Viable (MVP)** del sistema interno de gestión empresarial de **Universal de Cauchos Hurtado S.A.S.**  
Incluye los módulos de **Compras** y **Logística**, enfocados en la optimización operativa, la trazabilidad de procesos y la integración de información entre áreas clave.

El sistema demuestra la **viabilidad técnica** del proyecto, mejora la **eficiencia interna** y valida la **arquitectura escalable** planteada.

---

## ⚙️ Arquitectura del Sistema

El sistema está basado en el patrón **Modelo–Vista–Controlador (MVC)**, bajo el framework **ASP.NET Core (.NET 8)**:

📂 ProyectoMVP/
┣ 📁 Controllers/ → Lógica de control (manejo de flujo entre vistas y modelos)
┣ 📁 Models/ → Clases y entidades de negocio (EF Core)
┣ 📁 Views/ → Interfaz visual con Razor, Bootstrap y Chart.js
┣ 📁 wwwroot/ → Archivos estáticos (CSS, JS, imágenes)
┣ 📁 Migrations/ → Estructura de base de datos generada por EF
┣ 📁 Reports/ → Plantillas PDF y Excel
┣ 📄 appsettings.json → Configuración de conexión a SQL Server
┗ 📄 Program.cs → Configuración inicial del proyecto



---

## 🧩 Tecnologías Implementadas

| Capa | Tecnología / Herramienta | Descripción |
|------|----------------------------|--------------|
| **Frontend** | HTML5, CSS3, JavaScript, Bootstrap, Chart.js | Interfaz adaptable e interactiva |
| **Backend** | C#, ASP.NET MVC (.NET 8), Entity Framework | Lógica de negocio y conexión con BD |
| **Base de Datos** | SQL Server | Estructura relacional con triggers y stored procedures |
| **Reportes** | ClosedXML, Rotativa.AspNet | Exportación de datos a Excel y PDF |
| **Control de Versiones** | GitHub | Control de versiones colaborativo |
| **Metodología** | Scrum + Trello | Gestión ágil de tareas y sprints |

---

## 🧱 Módulos del MVP

### 🛒 Módulo de Compras
- Registro y validación de solicitudes.
- Control de aprobaciones y seguimiento.
- Exportación de reportes a Excel y PDF.
- Indicadores gráficos de rendimiento con Chart.js.

### 🚚 Módulo de Logística
- Gestión de novedades operativas y comerciales.
- Programación y seguimiento de rutas de entrega.
- Estadísticas de cumplimiento y rendimiento logístico.
- Integración directa con el módulo de compras.

---

## 📸 Imágenes del Sistema

### Pantalla principal del módulo de Compras  
![Vista Compras](./docs/images/compras.png)

### Seguimiento de Solicitudes  
![Tracking](./docs/images/tracking.png)

### Módulo de Logística  
![Logística](./docs/images/logistica.png)

> 📌 *Coloca tus capturas en la carpeta `/docs/images/` del repositorio.*

---

## 🚀 Cómo Exportar y Ejecutar el Proyecto

### 🔧 Requisitos Previos
- **Visual Studio 2022** o superior  
- **.NET 8 SDK** instalado  
- **SQL Server** (local o remoto)  
- **Git** para clonar el repositorio

---

### 🧭 Instrucciones

#### 1️⃣ Clonar el repositorio
```bash
git clone https://github.com/mchl27/ProyectoMVP.git


2️⃣ Abrir el proyecto

Abre el archivo ProyectoMVP.sln en Visual Studio.

3️⃣ Configurar la conexión a la base de datos

Edita el archivo appsettings.json con tu cadena de conexión:****
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=ProyectoMVP;User Id=USUARIO;Password=CONTRASEÑA;TrustServerCertificate=True;"
}

4️⃣ Actualizar la base de datos (si usas Entity Framework)

Ejecuta en la consola del administrador de paquetes:

5️⃣ Ejecutar el proyecto

Presiona F5 o selecciona Start Debugging.
El sistema se abrirá en tu navegador predeterminado.


💡 Características Clave

🔐 Autenticación y roles de usuario.

📦 Control de solicitudes con trazabilidad.

📊 Indicadores visuales de desempeño.

📁 Exportación de reportes (PDF / Excel).

💾 Integración con SQL Server.

🧠 Arquitectura modular y escalable.


🧠 Lecciones Aprendidas

Importancia de estructurar correctamente la arquitectura MVC desde el inicio.

Ventajas de aplicar metodologías ágiles con Trello y Scrum.

Relevancia de la documentación técnica y control de versiones con GitHub.

Optimización de consultas SQL y reducción de tiempos de carga.


📈 Próximos Pasos

🔧 Ajustes finales en validaciones y diseño.

📊 Implementación del Dashboard general con indicadores globales.

👥 Nuevos módulos: Comercial, Talento Humano, Contabilidad, Soporte Técnico.

🧾 Elaboración del manual de usuario y documentación técnica completa.



🧑‍💻 Desarrollado por

Michael Andres Lopez Cardenas - Equipo de desarrollo — Universal de Cauchos Hurtado S.A.S.
📍 Proyecto académico y corporativo
📬 Contacto: GitHub Repository
