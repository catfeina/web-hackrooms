#!/bin/sh

# Lista de climas posibles (en lugar de un array)
index=$(($RANDOM % 5))

case $index in
    1)
    weather="Llueve en la isla"
    ;;
    2)
    weather="Hay un tornado en el mar"
    ;;
    3)
    weather="Un huracán azota la isla"
    ;;
    4)
    weather="Isla parcialmente nublada"
    ;;
    *)
    weather="Buen clima en la playa"
    ;;
esac

# Determinar si es día o noche
minute=$(date +"%M")
if [ $(($minute % 7)) -eq 0 ]; then
    ligth="noche"
else
    ligth="día"
fi

# Actualizar el archivo del tiempo
echo "$weather y es de $ligth" > /usr/share/nginx/html/weather.txt
