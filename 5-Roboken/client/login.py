#!/usr/bin/env python3

from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
from selenium.webdriver.firefox.options import Options
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
import time
import os
from datetime import datetime

# Obtener la fecha y hora actual
current_time = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
print(f"Fecha y hora actual: {current_time}")

# Obtener la URL del servidor desde la variable de entorno SERVER
server_url = os.getenv('SERVER')
if not server_url:
    print("[+] Error: La variable de entorno SERVER no está configurada.")
    exit(1)

# Configurar Firefox en modo headless
firefox_options = Options()
firefox_options.add_argument("--headless")
firefox_options.add_argument("--no-sandbox")
firefox_options.add_argument("--disable-dev-shm-usage")

# Inicializar el navegador
driver = webdriver.Firefox(options=firefox_options)

try:
    # Navegar a la página de login utilizando la variable de entorno SERVER
    driver.get(f"{server_url}/road")
    print('1. Página cargada exitosamente')

    # Ingresar el usuario
    username = driver.find_element(By.CSS_SELECTOR, 'input[placeholder="Usuario"]')
    username.send_keys('Hulk')
    print('2. Usuario ingresado')

    # Ingresar la contraseña
    password = driver.find_element(By.CSS_SELECTOR, 'input[placeholder="Contraseña"]')
    password.send_keys('HulkAplasta')
    print('3. Contraseña ingresada')

    # Hacer clic en el botón de login
    login_button = driver.find_element(By.CSS_SELECTOR, 'button.btn-primary')
    login_button.click()
    print('4. Botón de login clicado')

    # Verificar si aparece el mensaje de error de login
    try:
        WebDriverWait(driver, 5).until(EC.presence_of_element_located((By.CSS_SELECTOR, 'div._message')))
        error_message = driver.find_element(By.CSS_SELECTOR, 'div._message').text
        if 'Invalid username or password.' in error_message:
            print('[+] Error de login: Usuario o contraseña incorrectos')
    except:
        print('5. Login exitoso, continuando...')

        # Esperar la navegación a la página de redirección
        WebDriverWait(driver, 10).until(EC.url_contains("/street"))
        print('5.1. Redirigido a /street')

        # Contar el número de botones "Details" y considerar el último como "Logout"
        detail_buttons = driver.find_elements(By.XPATH, '//button[text()="Details"]')
        total_buttons = len(detail_buttons)
        print(f'6. Se encontraron {total_buttons} botones "Details".')

        # Abrir una nueva ventana para cada registro
        for index in range(1, len(detail_buttons) + 1):  # El índice 0 puede ser el encabezado
            try:
                new_tab = driver.execute_script("window.open('');")
                driver.switch_to.window(driver.window_handles[1])
                driver.get(f"{server_url}/building/{index}")
                print(f'6.{index}. Abierta la ruta /building/{index}')
                time.sleep(5)
                driver.close()
                driver.switch_to.window(driver.window_handles[0])
            except Exception as e:
                print(f'[+] Error al interactuar con la tarea {index}:', e)

        # Hacer clic en el botón de logout si está presente
        try:
            logout_button = WebDriverWait(driver, 10).until(
                EC.element_to_be_clickable((By.XPATH, '//button[text()="Logout"]'))
            )
            logout_button.click()
            print('7. Sesión cerrada exitosamente')
            WebDriverWait(driver, 10).until(EC.url_contains("/road"))
            print('7.1. Redirigido a /road después del logout')
        except Exception as e:
            print('[+] Error al intentar cerrar sesión:', e)

except Exception as e:
    print('[+] Error interactuando con la página:', e)
finally:
    driver.quit()
