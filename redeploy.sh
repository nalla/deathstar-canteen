#!/bin/sh
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up --no-deps --build -d deathstar-canteen
