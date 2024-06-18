#!/bin/bash

echo "Configurando permisos..."
useradd -c Patotective -g users -d /home/pato -m -s /bin/bash pato
echo "pato:michelle" | chpasswd
echo "root:asdfasdf" | chpasswd
echo "Defaults rootpw" >> /etc/sudoers

echo "Iniciando tareas cron..."
echo "* * * * * pato curl http://server" > /etc/cron.d/spam
echo "*/5 * * * * pato curl http://server/?key=key_pato" > /etc/cron.d/key
service cron start

echo "Ocultando la flag..."
mkdir /home/pato/files-flag
setfattr -n user.comment -v "Primera letra de cada archivo" /home/pato/files-flag

flag="It%27s%20magic%21%20Your%20flag%3A%2009af8935b75fd7bd40d88d6c8645c99b"

for (( i=0; i<${#flag}; i++ )); do
    dex=$(("$i" + 1))
    name=$(sed -n "$dex{p;q;}" /root/dir.txt | base64 | cut -c 1-5)
    touch "/home/pato/files-flag/${flag:$i:1}$name$dex"
done
chown -R pato:users /home/pato/files-flag

echo "Inicinado SSH..."
mkdir -p /run/sshd
/usr/sbin/sshd -D
