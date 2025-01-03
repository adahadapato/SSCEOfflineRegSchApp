This C# data capture application use wpf architecture to create beautiful UI and the calssic .net framework.
It captures candidates registration information offline, it was intended for use in areas where there is limited or no internet connectivity.
Data is uploaded to online server when there is availability of internet connection.
Candidates are registered offline and the record stored on the local syste, in sqlite database, registration numbers are not assigned at this time,
this is done whe the data has been uploaded to the remote server. Yhe method of uniquely indentifying records on this system, is that each record is assigned a unique
identifyer, this identifyer is sent to the server, registration numbers are returned with these unique identifyers, to match those on the local system.
