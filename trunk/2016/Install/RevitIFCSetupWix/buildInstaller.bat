echo Build IFC installer

echo %1
echo %2
echo yifan knows it is not easy. 

set ThisBatFileRoot=%~dp0
rem Set this path to your Wix bin directory.
set WixRoot= "C:\Program Files (x86)\WiX Toolset v3.10\bin"

rem It is necessary to add the Wix bin directory to the system path temporarily to use the -ext flag below.
SET PATH=%PATH%;%WixRoot%

%WixRoot%\candle.exe -dProjectDir=%2 -ext WixUtilExtension %2Product.wxs 
%WixRoot%\light.exe -ext WixUtilExtension -out RevitIFC2016.msi product.wixobj -ext WixUIExtension

copy RevitIFC2016.msi %1..\Releasex64
del RevitIFC2016.msi

echo %1..\Releasex64\RevitIFC2016.msi
