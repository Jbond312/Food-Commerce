$t = $host.ui.RawUI.ForegroundColor
$host.ui.RawUI.ForegroundColor = "Green"

echo "Foods.Service..."
echo "Testing Cooks.Service..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

dotnet test Cooks.Service/Cooks.Service.Tests

$host.ui.RawUI.ForegroundColor = "Green"

echo "Finished testing Cooks.Service"
echo ""
$host.ui.RawUI.ForegroundColor = $t

echo "Testing Users.Service..."
echo ""
echo ""
$host.ui.RawUI.ForegroundColor = $t

dotnet test Users.Service/Users.Service.Tests

$host.ui.RawUI.ForegroundColor = "Green"

echo "Finished testing Users.Service"
echo ""
$host.ui.RawUI.ForegroundColor = $t
