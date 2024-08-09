#!/bin/sh

# Escribir todas las variables de entorno en un archivo que cron pueda leer
printenv | sed 's/^\(.*\)$/export \1/g' > /etc/environment

# Configurar el cron job
chmod 0644 /etc/cron.d/my-cron-job
crontab /etc/cron.d/my-cron-job

# Iniciar cron y mantener el contenedor activo
cron && tail -f /var/log/cron.log
