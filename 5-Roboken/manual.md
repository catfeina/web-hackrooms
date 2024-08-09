```bash
apk add docker
rc-update add docker
service docker start
```

```bash
nano /usr/local/bin/docker.sh
```

```script
#!/bin/sh
sleep 30

if [ "$(docker ps -a | wc -l)" != '1' ]; then
	docker rm -f $(docker ps -aq)
fi

docker system prune -fa

# Variables
ip=$(ip a show eth0 | grep inet | head -n 1 | cut -d ' ' -f 6)
subnet="$(ipcalc -n '$ip' | cut -d '=' -f 2)/$(ipcalc -p '$ip' | cut -d '=' -f 2)"
gateway=$(ip route | grep default | cut -d ' ' -f 3)
ipClient="$(echo -n '$ip' | cut -d '.' -f 1-3).56"

# Interfaces
ip link add name br0 type bridge
ip link set br0 up
ip link add link eth0 name eth0.10 type macvlan mode bridge
ip link set eth0.10 up
ip link set eth0.10 master br0
ip link set eth0 promisc on

# Containers
docker network create -d macvlan --subnet="$subnet" --gateway="$gateway" -o parent=eth0.10 client
docker network create internal

docker run -d --name api --network internal kradbyte/roboken:api
docker run -d --name server --network internal kradbyte/roboken:server
docker run -d --name reviewer --network internal kradbyte/roboken:reviewer
docker run -d --name user --network client --ip="$ipClient" -e SERVER="http://$(ipcalc -h '$ip' | cut -d '=' -f 1)" kradbyte/roboken:user
```

```bash
chmod 755 /usr/local/bin/docker.sh
echo "@reboot /usr/local/bin/docker.sh" >> /etc/crontabs/root
```
