const puppeteer = require('puppeteer');

(async () => {
  // Lanzar el navegador
  const browser = await puppeteer.launch({
    headless: true,
    args: [
      '--no-sandbox',
      '--disable-setuid-sandbox',
      '--ignore-certificate-errors',
      '--disable-web-security',
      '--disable-features=IsolateOrigins,site-per-process',
      '--disable-dev-shm-usage'
    ]
  });

  const page = await browser.newPage();

  // Ignorar errores SSL
  page.on('requestfailed', request => {
    console.log(`Request failed: ${request.url()} - ${request.failure().errorText}`);
  });

  try {
    // Navegar a la página de login
    await page.goto('http://app:4200/road', { waitUntil: 'networkidle2' });
    console.log('Página cargada exitosamente');
  } catch (error) {
    console.error('Error navegando a la página:', error);
  }

  try {
    // Ingresar el usuario
    await page.type('input[placeholder="Usuario"]', 'tu_usuario');
    console.log('Usuario ingresado');

    // Ingresar la contraseña
    await page.type('input[placeholder="Contraseña"]', 'tu_contraseña');
    console.log('Contraseña ingresada');

    // Hacer clic en el botón de login
    await page.click('button.btn-primary');
    console.log('Botón de login clicado');

    // Esperar la navegación a la página de redirección
    await page.waitForNavigation();
    console.log('Login exitoso, redirigido a /street');
  } catch (error) {
    console.error('Error interactuando con la página:', error);
  }

  await browser.close();
})();
