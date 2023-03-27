$solution_dir = (gi .).Parent.FullName
$git = .\git
$cloc= $solution_dir + "cloc.exe"
$processOptions = @{
    FilePath = "git"
    ArgumentList = "rev-parse --short HEAD"
    WorkingDirectory = $solution_dir
    WindowStyle = Hidden
}
$id = Start-Process @processOptions
$output = $id+ ".txt"
$cmd = $id + "--report-file"+ $id + ".txt"
Start-Process -FilePath $cloc -ArgumentList $cmd