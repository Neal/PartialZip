# PartialZip
Download files from a remote ZIP archive using [`RemoteZipFile.cs`](https://github.com/Neal/PartialZip/blob/master/RemoteZip.cs) and [`SharpZipLib`](http://sharpziplib.com/).


## Usage

    PartialZip.exe <ZipURL> <ZipFilePath> <LocalFilePath>

### Examples

    PartialZip.exe "http://apple.com/iPod4,1_5.1.1_9B206_Restore.ipsw" "Firmware/dfu/iBSS.n81ap.RELEASE.dfu" iBSS.n81ap
    PartialZip.exe "http://ineal.me/pie.zip" "apple.pie" "C:\Users\Neal\Desktop\APPLEPIE"
