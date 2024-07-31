# Apinje

- [1. Descripción](#1-descripción)
- [2. Instalación y despliegue](#2-instalación-y-despliegue)
- [2.1. Despliegue automático](#21-despliegue-automático)
- [2.2. Despliegue manual](#22-despliegue-manual)
- [2.3. Observaciones](#23-observaciones)
- [3. Descripción del laboratorio](#3-descripción-del-laboratorio)
- [4. Resolución de la máquina](#4-resolución-de-la-máquina)
- [4.1. Descripción general](#41-descripción-general)

## 1. Descripción

Este laboratorio está centrado en explotar vulnerabilidades mencionadas en el [Owasp Top 10](https://owasp.org/www-project-top-ten). Los usuarios podrán practicar habilidades de captura de solicitudes, inspecciones de contenido JS, errores de encriptación e inyecciones SQL. Este laboratorio utiliza dos imágenes: el servidor está basada en Nginx:Alpine, que contiene la SPA está construida con Angular 18.1.1, y la Api está construida con .Net Core 8.

## 2. Instalación y despliegue

Para el despliegue de contenedores será necesario crear una red de tipo `bridge`:

```bash
docker network create apinje
```

### 2.1. Despliegue automático

Para desplegar el laboratorio, basta con ejecutar los siguientes comandos:

```bash
docker run -d --name api --network apinje -e ASPNETCORE_ENVIRONMENT=Production kradbyte/apinje:api
docker run -d --name server -p 80:80 --network apinje kradbyte/apinje:server
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

En caso de modificar la API, también hay un [dockerfile](minimalapi/Dockerfile) que servirá para compilar nuestra API. A diferencia del Dockerfile anterior, con la API solo podremos generar una imagen compilada, para pruebas deberemos instalar Visual Studio o eliminar y reconstruir la imagen por cada cambio que hagamos.

```bash
#cd mvcapi
docker build -t api .
```

- Por último lanzamos los contenedores con nuestras imágenes creadas.

```bash
docker run -d --name api --network apinje -e ASPNETCORE_ENVIRONMENT=Production api
docker run -d --name server -p 80:80 --network apinje server
```

En caso de querer modificar la base de datos, deberemos dirigirnos a la carpeta [data](mvcapi/data) y lanzar alguna herramienta para SQLite o trabajar directamente con un contenedor:

```bash
# cd mvcapi/data
docker run -it --rm --name sqlite -v $(pwd):/data -w /data nouchka/sqlite3 mydatabase.db
```

> La base de datos se copia dentro del contenedor al momento de la construcción de la API, por lo que no es persistente, a menos que se le monte un volumen.

### 2.3. Observaciones

- El único nombre del contenedor que no puede variar es el de la api, ya que la configuración internta de la SPA resuelve el DNS para consumir los endpoints. Se puede cambiar el nombre del contenedor, modificando el archivo [proxy.conf.json](app/src/proxy.conf.json) y compilando la aplicación, de igual modo hay que modificar el archivo [defult.conf](default.conf) con el nuevo nombre del contenedor de la api.

- Si tenemos el puerto 80 ocupado por otro servicio, no habrá problema con mapear dicho puerto con otro.

```bash
docker run -d --name api --network apinje -e ASPNETCORE_ENVIRONMENT=Production api
docker run -d --name server -p [NewHostPort]:80 --network apinje server
```

- Si se llegase a tener problemas de cors al consumir la API, podemos agregar la IP a los orígenes permitidos de la API, con la variable de entorno `OriginsCors`.

```bash
docker run -d --name api --network apinaje -e ASPNETCORE_ENVIRONMENT=Production -e OriginsCors=http://[IpClient]:[PortClient] kradbyte/apinaje:api
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
