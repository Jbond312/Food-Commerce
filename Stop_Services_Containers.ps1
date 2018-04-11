cd Foods.Service

$t = $host.ui.RawUI.ForegroundColor

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Stopping existing Foods.Service containers..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

docker-compose stop

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Foods.Service containers stopped..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

cd ..

cd Identity.Service

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Stopping existing Identity.Service containers..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

docker-compose stop

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Identity.Service containers stopped..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

cd ..

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo "All containers stopped"
$host.ui.RawUI.ForegroundColor = $t