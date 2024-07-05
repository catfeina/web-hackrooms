# Usa la imagen base de nginx-alpine
FROM nginx:alpine

# Copia los archivos compilados de Angular a la carpeta de Nginx
COPY ./app/dist/app /usr/share/nginx/html
COPY default.conf /etc/nginx/conf.d/default.conf

# Exponer el puerto 80
EXPOSE 80

# Iniciar Nginx
CMD ["nginx", "-g", "daemon off;"]
