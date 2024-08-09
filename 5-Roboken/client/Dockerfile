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

RUN pip install selenium

WORKDIR /app
ENV SERVER=http://server

COPY login.py .
RUN chmod 755 login.py

COPY crontab /etc/cron.d/my-cron-job
RUN chmod 0644 /etc/cron.d/my-cron-job
RUN crontab /etc/cron.d/my-cron-job
RUN mkdir -p /var/log && touch /var/log/cron.log

CMD ["sh", "-c", "cron && tail -f /var/log/cron.log"]
