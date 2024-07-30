FROM node:22.4.1
RUN npm install -g @angular/cli@18.1.0
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build
EXPOSE 4200
CMD ["npm", "start"]
