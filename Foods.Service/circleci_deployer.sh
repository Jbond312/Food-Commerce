export BRANCHNAME=$1
export LINODEIP=$2
docker-compose -f $1-docker-compose.yml stop; 
docker-compose -f $1-docker-compose.yml rm -f; 
docker-compose -f $1-docker-compose.yml up -d; 
docker images --quiet --filter=dangling=true | xargs --no-run-if-empty docker rmi -f