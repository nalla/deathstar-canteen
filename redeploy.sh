#!/bin/sh
docker-compose build deathstar-canteen
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up --no-deps -d deathstar-canteen
