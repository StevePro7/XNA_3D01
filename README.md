# XNA_3D01

07/12/2019
Trying to get the following to build + run
C:\Steven\XNA\Learning\02_3D_MG\02_3D_MG\Content\bin\Windows

However, got ContentLoadException
therefore add existing 2x XNB files and set build action to Copy
robot.xnb
robottexture.xnb

But now get error:
An unhandled exception of type 'System.AccessViolationException' occurred in SharpDX.Direct3D11.dll



02.
Import ship.fbx
Importer FbxImporter had unexpected failure
AssImp.AssImpException
Error importing FIL_DCOM unsupported old format

Installed
fbx20133_converter_win_x64.exe
C:\Steven\XNA\MonoGame3D

Installed software but must learn how to convert FBX


03.
https://darkgenesis.zenithmoon.com/updating-old-fbx-files-for-the-modern-era

Launch Content.mgcb
Copy ship_tex.tga to Content directory

FBX Converter
https://www.autodesk.com/developer-network/platform-technologies/fbx-converter-archives
FBX 2013.3 Converter for Windows
Add FBX Converter
Add...
Copy fbx file to temp folder
E:\Steven\Temp\4
Destination file location
Change destination folder
point to same Content directory
Embed data
FBX Save Mode	binary
Convert

This will save the FBX2013 converted ship.fbx file next to ship_tex.tga
right click ship.fbx
Rebuild
OK

Tried again but dump both fbx + tga files into Content directory
Convert but set destination same as source but just rename e.g. ship_cv1
so at least on the same level
then rename to ship_cv1 in code


SUMMARY
the old programs work
is this because
they are separate proj / sln file per platform
they are using an older version of MG package	3.7.0

Source:
D:\Steven\XNA\BOOK2\XNA 3D\0041OT_Code\Code Bundle\0041_01_revised code\1_LoadModel