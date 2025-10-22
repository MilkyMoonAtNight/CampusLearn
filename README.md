How to create and run the database on Docker:
(Note: the containers should already be created)
Inside the project terminal run : docker cp "YOUR PATH TO THE SQL FILE" campuslearn-db:/campuslearn_schema.sql
Then run: docker exec -it campuslearn-db psql -U postgres -d campuslearn -f /campuslearn_schema.sql
