param(
    [string]$Dll,
    [string]$Proj
)

$projDir = Split-Path $Proj -Parent
$outDir  = Split-Path $Dll -Parent
$name    = [IO.Path]::GetFileNameWithoutExtension($Dll)
$dest    = Join-Path $env:APPDATA "Autodesk\Revit\Addins\2024"

New-Item -ItemType Directory -Force $dest | Out-Null

# Lấy tên nhánh: ưu tiên đọc .git\HEAD, không phụ thuộc lệnh git
$branch = "no-git"
$dir = $projDir
while ($dir) {
    $headFile = Join-Path $dir ".git\HEAD"
    if (Test-Path $headFile) {
        $head = (Get-Content $headFile -Raw).Trim()
        if ($head -match '^ref:\s*refs/heads/(.+)$') { $branch = $Matches[1] }
        elseif ($head.Length -ge 7) { $branch = $head.Substring(0, 7) }
        break
    }
    $dir = Split-Path $dir -Parent
}
$branch = $branch -replace '[\\/]', '-'

# Ghi đè DLL và PDB
Copy-Item (Join-Path $outDir "$name.dll") $dest -Force
Copy-Item (Join-Path $outDir "$name.pdb") $dest -Force

# Tạo .addin từ file mẫu
$tpl = Get-Content (Join-Path $projDir "$name.addin.template") -Raw -Encoding UTF8
$tpl.Replace("__BRANCH__", $branch) |
    Set-Content (Join-Path $dest "$name.addin") -Encoding UTF8

Set-Content (Join-Path $dest "branch.txt") $branch -Encoding UTF8

Write-Host "Deployed branch '$branch' to $dest"
