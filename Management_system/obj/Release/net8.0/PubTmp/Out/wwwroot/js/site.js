// Obtener elementos necesarios del DOM
const readMoreLinks = document.querySelectorAll('.read-more');
const modals = document.querySelectorAll('.modal');
const closeButtons = document.querySelectorAll('.close');

// Agregar evento click a cada botón "Leer más"
readMoreLinks.forEach((link, index) => {
    link.addEventListener('click', (event) => {
        event.preventDefault(); // Evitar que el enlace abra una nueva página
        modals[index].style.display = 'block';
    });
});

// Agregar evento click a cada botón de cierre
closeButtons.forEach((button, index) => {
    button.addEventListener('click', () => {
        modals[index].style.display = 'none';
    });
});