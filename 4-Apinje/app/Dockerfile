FROM node:22.5.1
RUN npm install -g @angular/cli@18.1.1
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build
EXPOSE 4200
CMD ["npm", "start"]
