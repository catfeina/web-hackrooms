FROM python:3.12-slim

# Instalar dependencias necesarias
RUN apt-get update && apt-get install -y \
    wget \
    curl \
    gnupg \
    unzip \
    firefox-esr=115.14.0esr-1~deb12u1 \
    cron \
    --no-install-recommends \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Bloquear la versión de Firefox ESR para evitar actualizaciones automáticas
RUN apt-mark hold firefox-esr

# Descargar e instalar GeckoDriver 0.35.0
RUN wget -q -O /tmp/geckodriver.tar.gz https://github.com/mozilla/geckodriver/releases/download/v0.35.0/geckodriver-v0.35.0-linux64.tar.gz \
    && tar -xzf /tmp/geckodriver.tar.gz -C /usr/local/bin/ \
    && rm /tmp/geckodriver.tar.gz

# Instalar Selenium
RUN pip install selenium
WORKDIR /app
ENV SERVER=http://server

# Copiar el script de Python
COPY login.py .
RUN chmod 755 login.py

# Copiar el archivo de cron
COPY crontab /etc/cron.d/my-cron-job
RUN chmod 0644 /etc/cron.d/my-cron-job

# Copiar el script de entrada
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh
RUN mkdir -p /var/log && touch /var/log/cron.log

# Establecer el punto de entrada
ENTRYPOINT ["/app/entrypoint.sh"]
