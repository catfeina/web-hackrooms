FROM python:3.12-slim

# Instalar dependencias necesarias
RUN apt-get update && apt-get install -y \
    wget \
    curl \
    gnupg \
    unzip \
    firefox-esr \
    cron \
    --no-install-recommends \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Descargar e instalar GeckoDriver 0.35.0
RUN wget -q -O /tmp/geckodriver.tar.gz https://github.com/mozilla/geckodriver/releases/download/v0.35.0/geckodriver-v0.35.0-linux64.tar.gz \
    && tar -xzf /tmp/geckodriver.tar.gz -C /usr/local/bin/ \
    && rm /tmp/geckodriver.tar.gz

# Instalar Selenium
RUN pip install selenium

# Crear directorio de trabajo
WORKDIR /app

# Copiar el script de login
COPY login.py .
RUN chmod 755 login.py

# Copiar crontab
COPY crontab /etc/cron.d/my-cron-job

# Dar permisos a crontab
RUN chmod 0644 /etc/cron.d/my-cron-job

# Aplicar crontab
RUN crontab /etc/cron.d/my-cron-job

# Crear el directorio y archivo de log
RUN mkdir -p /var/log && touch /var/log/cron.log

# Comando para ejecutar cron y el script
CMD ["sh", "-c", "cron && tail -f /var/log/cron.log"]
