document.addEventListener('DOMContentLoaded', function () {
    const currencyInputs = document.querySelectorAll('input[data-format="currency"]');

    currencyInputs.forEach(input => {
        input.addEventListener('input', (event) => {
            let value = input.value.replace(/[^0-9]/g, ''); // Solo números
            if (value) {
                value = parseFloat(value).toLocaleString('es-CL', { // Configuración regional
                    style: 'currency',
                    currency: 'CLP',
                    minimumFractionDigits: 0
                });
                input.value = value.replace('CLP', '').trim(); // Remueve el CLP si no lo deseas
            }
        });

        input.addEventListener('focusout', (event) => {
            let value = input.value.replace(/[^0-9]/g, '');
            if (value) {
                input.value = parseFloat(value).toLocaleString('es-CL', {
                    style: 'currency',
                    currency: 'CLP',
                    minimumFractionDigits: 0
                });
            }
        });
    });
});