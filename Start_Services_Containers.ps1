cd Foods.Service

$t = $host.ui.RawUI.ForegroundColor

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Starting existing Foods.Service containers..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

docker-compose start

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Foods.Service containers started..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

cd ..

cd Identity.Service

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Starting existing Identity.Service containers..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

docker-compose start

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo ""
echo "Identity.Service containers started..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

cd ..

$host.ui.RawUI.ForegroundColor = "Green"
echo ""
echo "All containers started"
$host.ui.RawUI.ForegroundColor = $t