# Configuración manual de servidor

## 1. Instalación de docker

```bash
apk add docker
rc-update add docker
service docker start
```

> **Nota**: Comando para Alpine Linux. Ajustar según distro.

## 2. Creación de script de despliegue

```bash
nano /usr/local/bin/docker.sh
```

## 3. Script de despliege

Dentro del script creado anteriormente copiar el siguiente comando, ajustar las IP si estas no están disponibles en red de trabajo o cambiar el adaptador según sea el caso:

```script
#!/bin/sh
sleep 30

if [ "$(docker ps -a | wc -l)" != '1' ]; then
	docker rm -f $(docker ps -aq)
fi

docker system prune -fa

# Variables
ip=$(ip a show eth0 | grep inet | head -n 1 | cut -d ' ' -f 6)
subnet="$(ipcalc -n "$ip" | cut -d '=' -f 2)/$(ipcalc -p "$ip" | cut -d '=' -f 2)"
gateway=$(ip route | grep default | cut -d ' ' -f 3)
clientIp="$(echo -n "$ip" | cut -d '.' -f 1-3).56"
serverIp="$(echo -n "$ip" | cut -d '.' -f 1-3).57"

# Containers
docker network create -d ipvlan --subnet="$subnet" --gateway="$gateway" -o parent=eth0 external
docker network create internal

docker pull kradbyte/roboken:reviewer
docker pull kradbyte/roboken:user
docker pull kradbyte/roboken:api
docker pull kradbyte/roboken:server

docker run -d --name api --network internal kradbyte/roboken:api
docker run -d --name server --network external --network-alias=private --ip="$serverIp" kradbyte/roboken:server
docker run -d --name reviewer --network internal kradbyte/roboken:reviewer
docker run -d --name user --network external --ip="$clientIp" -e SERVER="http://$(ipcalc -h '$serverIp' | cut -d '=' -f 1)" kradbyte/roboken:user
```

## 4. Programación de tarea

```bash
chmod 755 /usr/local/bin/docker.sh
echo "@reboot /usr/local/bin/docker.sh" >> /etc/crontabs/root
```
