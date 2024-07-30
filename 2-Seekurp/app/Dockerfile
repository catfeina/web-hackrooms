FROM node:22.4.1
RUN npm install -g @angular/cli@18.0.7
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build
EXPOSE 4200
CMD npm start
