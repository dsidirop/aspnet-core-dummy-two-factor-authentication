$folders = @(
"bin",
"obj",
".axoCover",
".vs",
"packages",
"node_modules"
);

gci -force -include $folders -recurse -Attributes !Hidden, !System, !ReparsePoint | remove-item -force -recurse;
