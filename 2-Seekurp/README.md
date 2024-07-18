# Seekurp

- [1. Descripción](#1-descripción)
- [2. Instalación y despliegue](#2-instalación-y-despliegue)
- [2.1. Despliegue automático](#21-despliegue-automático)
- [2.2. Despliegue semiautomático](#22-despliegue-semiautomático)
- [2.3. Despliegue manual](#23-despliegue-manual)
- [3. Descripción del laboratorio](#3-descripción-del-laboratorio)
- [4. Resolución de la máquina](#4-resolución-de-la-máquina)
- [4.1. Camino de la felicidad](#41-camino-de-la-felicidad)
- [4.2. Camino del dolor](#42-camino-del-dolor)

## 1. Descripción

Esta imagen facilita un laboratorio centrado en inspecciones de contenido de una SPA. Los usuarios podrán practicar habilidades de captura de solicitudes e inspecciones de contenido JS. Este laboratorio utiliza una imagen basada en Nginx:Alpine y la SPA está construida con Angular 8.

## 2. Instalación y despliegue

### 2.1. Despliegue automático

Para desplegar el laboratorio, basta con descargar la máquina desde este [enlace](https://www.mediafire.com/file/a1rl6b278manprp/Seekurp.zip/file). Una vez descargada, debemos descomprimirla y hacer doble clic para importarla a VirtualBox (por el momento, la única plataforma soportada). Una vez importada, basta con iniciar la máquina y, cuando aparezca el mensaje de login, podremos comenzar a trabajar.

![Login](imagine/login.png)

### 2.2. Despliegue semiautomático

Para desplegarla manualmente bastará con ejecutar el siguiente comando:

```bash
docker run -d --name seekurp -p 80:80 kradbyte/seekurp:latest
```

> **Nota**: La diferencia entre utilizar la máquina virtual y el contenedor es únicamente la IP que utilizaremos para acceder al laboratorio.

### 2.3. Despliegue manual

Para desplegar manualmente el laboratorio bastará con construir la imagen y lanzar el contenedor, ya sea en una máquina virtual o mapeando los puertos para que sea accedible por `localhost`. Puedes cambiar el funcionamiento de la SPA en la carpeta [app](app), posteriormente deberás compilar la aplicación y construir la imagen desde esta carpeta, utilizando el [Dockerfile](Dockerfile).
Dentro de la carpeta [app](app) se encuentra un Dockerfile que puede ser utilizado para probar la aplicación, en caso no tengas o quieras instalar Node en tu ordenador, para ello deberemos ingresar a la carpeta y seguir estos pasos:

- Construir la imagen con Node para la compilación.

```bash
docker build -t app .
```

- Lanzamos el contenedor para compilar o modificar la SPA.

```bash
docker run -d --name app -p 4200:4200 -v $(pwd):/app -w /app app
```

- Si modificamos la SPA y queremos revisar nuestros cambios, será necesario ejecutar el siguiente comando para precompilar el código.

```bash
docker exec -it app bash
ng serve --host 0.0.0.0 --disable-host-check
```

Luego podremos ingresar al navegador para verificar nuestros cambios, en `http://localhost:4200`.

- Una vez hayamos verificado nuestros cambios y que estos sean funcionales, deberemos compilar el código:

```bash
docker exec -it app bash
ng build --configuration production
```

- Para limpiar nuestro sistema podremos limpiar caché y las construccioes que Docker almacena, y por último podremos constuir nuestra imagen con el (Dockerfile)[Dockerfile] principal.

```bash
docker system prune -fa
#cd 2-Seekurp
docker build -t server .
docker run -d --name server -p 80:80 server
```

## 3. Descripción del laboratorio

**Description**
Después de buscar por las cavernas lograste salir hacia el nivel 6, ahora, dentro del nuevo espacio, debes buscar la salida para avanzar (obvio). Es probable que debas atrapar algo para salir de aquí, busca bien y procura no perderte en la oscuridad.

**Target**
Atrápalo y úsalo a tu favor.

**Steps**
- Busca pistas.
- Atrápalo 
- Obtén la flag

**Tools**
BurpSuite/wfuzz/others-tools/customs-scripts

**Grants**
[common.txt](https://github.com/danielmiessler/SecLists/blob/master/Discovery/Web-Content/common.txt)

**Run**
1. Descargar la máquina desde el [enlace](https://www.mediafire.com/file/a1rl6b278manprp/Seekurp.zip/file).
2. Descomprimir el archivo y al dar doble clic se importará la máquina a VirtualBox.
3. Para saber si el laboratorio ya está operativo, puedes acceder a la IP de la máquina que se lanzó o con localhost, según se configuró, desde el navegador (desde que se lanza la máquina habrá que esperar unos dos minutos para que todas las configuraciones carguen).
4. Dependiendo de la conexión a Internet, deberás verificar la configuración de red de la máquina: Dual-band para WiFi y PCIe para conexiones por cable.

![Adaptadores](imagine/adapters.png)

5. Una vez cargue la siguiente pantalla, la máquina estará encendida y podrás comenzar a trabajar en el laboratorio. Ten en cuenta que no accederás al servidor por esta interfaz; esta pantalla solo indica que el laboratorio está operativo.

![Login](imagine/login.png)

## 4. Resolución de la máquina

### 4.1. Camino de la felicidad

Filtra la flag en el archivo `main.js`.

### 4.2. Camino del dolor

- Enumera las rutas con alguna herramienta o verificando el `main.js`.
- Inspecciona las rutas y encuentra la protegida.
- Captura la solicitud y modifíca el parámetro.
- Captura la flag.
