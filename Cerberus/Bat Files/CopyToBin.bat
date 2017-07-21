REM Copy Wagering executables to the CerberusDir\bin directory
REM Add to VS External Tools with Arguments: Debug D:, Initial Directory: $(SolutionDir) 

del %2\CerberusDir\bin\*.* /Q

REM Host -> Bin
copy ..\bin\%1\*.exe %2\CerberusDir\bin\*.*
copy ..\bin\%1\*.dll %2\CerberusDir\bin\*.*
copy ..\bin\%1\*.config %2\CerberusDir\bin\*.*
copy ..\bin\%1\*.pdb %2\CerberusDir\bin\*.*

REM Support Tools -> Support
del %2\CerberusDir\Support\*.* /Q
copy ..\..\Cerberus.Admin\bin\%1\*.exe %2\CerberusDir\Support\*.*
copy ..\..\Cerberus.Admin\bin\%1\*.dll %2\CerberusDir\Support\*.*
copy ..\..\Cerberus.Admin\bin\%1\*.config %2\CerberusDir\Support\*.*
copy ..\..\Cerberus.Admin\bin\%1\*.pdb %2\CerberusDir\Support\*.*
