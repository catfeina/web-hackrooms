#!/bin/sh

# Instala Docker e iniciar el servicio
apk update
apk add docker
rc-update add docker boot
service docker start

# Script para configurar contenedores
script="/usr/local/bin/docker.sh"
echo "#!/bin/sh" > $script
echo "sleep 30" >> $script
echo "nets=$(docker network ls | grep mcpato | wc -l)" >> $script
echo "conts=$(docker ps -a | wc -l)" >> $script
echo "inter=$(ip a | grep br0 | wc -l)" >> $script
echo "subnet='192.168.0.0/24'" >> $script
echo "gateway='192.168.0.1'" >> $script
echo "if [ '$conts' != '0' ]; then" >> $script
echo "	docker rm -f $(docker ps -aq)" >> $script
echo "fi" >> $script
echo "if [ '$nets' != '0' ]; then" >> $script
echo "	docker network rm mcpato" >> $script
echo "fi" >> $script
echo "if [ '$inter' != '0' ]; then" >> $script
echo "	ip link del eth0.10" >> $script
echo " 	ip link del br0" >> $script
echo "fi" >> $script
echo "ip link add name br0 type bridge" >> $script
echo "ip link set br0 up" >> $script
echo "ip link add link eth0 name eth0.10 type macvlan mode bridge" >> $script
echo "ip link set eth0.10 up" >> $script
echo "ip link set eth0.10 master bro0" >> $script
echo "ip link set eth0 promisc on" >> $script
echo "docker network create -d macvlan --subnet='$subnet' --gateway='$gateway' -o parent=eth0.10 mcpato" >> $script
echo "docker run -d --name server --network mcpato --ip=192.168.0.90 kradbyte/vmitmy:server" >> $script
echo "docker run -d --name client --network mcpato --ip=192.168.0.91 kradbyte/vmitmy:client" >> $script
chmod 755 $script

# Programación de tarea cron
echo "@reboot $script" >> /etc/crontabs/root

# Creación de usuario
useradd -c "Miau-miau" -g users -d /home/miua -m -s /bin/sh miau
echo "miau:tits" | chpasswd
chown -R root:root /home/miau

# Binario cave
script="/usr/bin/cave"
echo "#!/bin/sh" > $script
echo "if [ '$1' == 'explore' ]; then" >> $script
echo "	echo 'Explorando la isla...'" >> $script
echo "	sleep 5" >> $script
echo "	echo 'Cave 1: No se pudo explorar logs, verificar manualmente. :c'; sleep 3" >> $script
echo "	echo 'Cave 2: /etc/network'; sleep 3" >> $script
echo "	echo 'Cave 3: /tmp/black-hole'" >> $script
echo "else" >> $script
echo "	echo 'Tarea o reconocida. Explora el lugar. :3'" >> $script
echo "fi" >> $script
chmod 755 $script
echo "cave explore" >> /home/miau/.ash_history
echo "Los últimos números indican el ordenamiento de menor a mayor." > /var/log/black-hole
echo "Para los nombres cortos, solo el último número indica el ordenamiento" >> /var/log/black-hole
echo "Concatenar solamente el primer caracter y seguir el ordenamiento..." >> /etc/network/history

# Desintegración de flag
flag="It%27s%20magic%21%20Your%20flag%3A%2009af8935b75fd7bd40d88d6c8645c99b"
mkdir /home/miau/file-broke
for i in $(seq 0 $((${#flag} - 1))); do
    dex=$(($i + 1))
    name=$(sed "${dex}q;d" /root/dir.txt | base64 | cut -c 1-5)
    touch "/home/miau/file-broke/${flag:$i:1}$name$dex"
done
