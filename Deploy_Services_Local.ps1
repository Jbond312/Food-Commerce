cd MongoReadyRoll

$t = $host.ui.RawUI.ForegroundColor
$host.ui.RawUI.ForegroundColor = "Green"

echo "MongoReadyRoll..."
echo "Building MongoReadyRoll..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

dotnet restore
dotnet build
dotnet publish . -c Release -o ./obj/Docker/publish

cd ..
cd Foods.Service

$host.ui.RawUI.ForegroundColor = "Green"

echo "Foods.Service..."
echo "Building Foods.Service..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

dotnet restore
dotnet build
dotnet publish . -c Release -o ./obj/Docker/publish

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Dropping existing Foods.Service containers..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

docker-compose stop
docker-compose rm -f

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Building and deploying Foods.Service containers..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

docker-compose build
docker-compose up -d

cd ..

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Finished deploying containers"
$host.ui.RawUI.ForegroundColor = $t