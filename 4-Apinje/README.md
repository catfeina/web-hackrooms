# Apinje

- [1. Descripción](#1-descripción)
- [2. Instalación y despliegue](#2-instalación-y-despliegue)
- [2.1. Despliegue automático](#21-despliegue-automático)
- [2.2. Despliegue manual](#22-despliegue-manual)
- [3. Descripción del laboratorio](#3-descripción-del-laboratorio)
- [4. Resolución de la máquina](#4-resolución-de-la-máquina)
- [4.1. Descripción general](#41-descripción-general)

## 1. Descripción

Este laboratorio está centrado en explotar vulnerabilidades mencionadas en el [Owasp Top 10](https://owasp.org/www-project-top-ten). Los usuarios podrán practicar habilidades de captura de solicitudes, inspecciones de contenido JS, errores de encriptación e inyecciones SQL. Este laboratorio utiliza dos imágenes: el servidor está basada en Nginx:Alpine, que contiene la SPA está construida con Angular 18, y la Api está construida con .Net Core 8.

## 2. Instalación y despliegue

### 2.1. Despliegue automático

Para desplegar el laboratorio, basta con ejecutar los siguientes comandos:

```bash
docker run -d --name api -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Production kradbyte/apinje:api
docker run -d --name server -p 80:80 kradbyte/apinje:server
```

### 2.2. Despliegue manual

Para desplegar manualmente el laboratorio bastará con construir la imagen y lanzar los contenedores, ya sea en una máquina virtual o mapeando los puertos para que sea accedible por `localhost`. Puedes cambiar el funcionamiento de la SPA en la carpeta [app](app), posteriormente deberás compilar la aplicación y construir la imagen desde esta carpeta, utilizando el [Dockerfile](Dockerfile).
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
#cd 4-Apinje
docker build -t server .
```

En caso de querer modificar la API, también hay un [dockerfile](minimalapi/Dockerfile) que servirá para compilar nuestra API. A diferencia del Dockerfile anterior, con la API solo podremos generar una imagen compilada, para pruebas deberemos instalar Visual Studio o eliminar y reconstruir la imagen por cada cambio que hagamos.

```bash
#cd mvcapi
docker build -t api .
```

Por último lanzamos los contenedores con nuestras imágenes creadas.

```bash
docker run -d --name server -p 80:80 server
docker run -d --name api -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Production api
```

> **Nota**: Si tenemos el puerto 80 ocupado por otro servicio, podremos modificar la variable `OriginsCors` para no tener problemas de Cors en la API. Si tenemos el puerto 8080 ocupado sí tendremos problemas en la SPA...por el momento solo se puede solucionarlo modificando el archivo [environments.ts](app/src/app/environments/environments.ts) y posteriormente compilando la SPA y construyendo la imagen con el [Dockerfile](Dockerfile).

```bash
docker run -d --name server -p [NewPort]:80 kradbyte/apinje:server
docker run -d --name api -p 8080:8080 -e ASPNETCORE_ENVIRONMENT -e OriginsCors=http://localhost:[NewPort] kradbyte/apinje:api
```

## 3. Descripción del laboratorio

**Description**
Owasp es una organización sin fines de lucro que reúne cada cierto período de tiempo, las vulnerabilidades más comunes de las aplicaciones Web. Para este lab, emularás ser parte del red team para encontrar algunas de las 10 vulnerabilidades más comunes de las aplicaciones Web.

**Target**
Recupera la flag

**Steps**
- Auditoría general de funcionamiento
- Búsqueda de rutas y endpoints
- Exploración de vulnerabilidades Owasp
- Recuperación de flag

**Grants**
- MD5, [500-worst-passwords.txt](https://github.com/danielmiessler/SecLists/blob/master/Passwords/Common-Credentials/500-worst-passwords.txt)
- [2021](https://owasp.org/www-project-top-ten/): A02, A03 y A07

**Tools**
BurpSuite, Postman/curl, Development tools (navigator), John the Ripper/custom scripts.

## 4. Resolución de la máquina

### 4.1. Descripción general

Podremos inspeccionar el archivo `main.js` para verificar las rutas o podremos explorar la SPA manualmente para ver el funcionamiento general del sitio. Al explorar las rutas y funcionalidades de la forma, encontraremos una ruta cuyo parámetro es vulnerable a SQL Injection (poem). Cuando hayamos obtenido los usuarios y las contraseñas, notaremos que las contraseñas están hasheadas con MD5, por lo que podremos utilizar `John The Ripper`, o algún script personalizado, para obtener la contraseña en texto claro. Cuando nos hayamos logueado nos redirijirá a la lectura de la flag.
