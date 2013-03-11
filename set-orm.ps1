param([string] $orm = "nh")

Get-ChildItem -Recurse -Filter "Web.*csproj" | % {
    Write-Host $_.Name
    
    $filename = $_.Fullname
     
    $proj = [xml]( Get-Content $_.Fullname )
 
	$ns = "http://schemas.microsoft.com/developer/msbuild/2003"
 
    $xmlNameSpace = new-object System.Xml.XmlNamespaceManager($proj.NameTable)
 
    $xmlNameSpace.AddNamespace("p", $ns)
    
	
	$hintPath = $proj.SelectSingleNode("/p:Project/p:ItemGroup/p:Reference[contains(@Include, 'HibernatingRhinos.Profiler.Appender')]", $xmlNameSpace).get_ChildNodes().Item(0)
	$efFile = $proj.SelectSingleNode("/p:Project/p:ItemGroup/p:None[contains(@Include, 'EntityFrameworkProfilerBootstrapper')]", $xmlNameSpace)
	
	if($efFile -eq $null){
		$efFile = $proj.SelectSingleNode("/p:Project/p:ItemGroup/p:Compile[contains(@Include, 'EntityFrameworkProfilerBootstrapper')]", $xmlNameSpace)
	}
	
	$efFile2 = $proj.SelectSingleNode("/p:Project/p:ItemGroup/p:None[contains(@Include, 'EntityFrameworkModule')]", $xmlNameSpace)
	
	if($efFile2 -eq $null){
		$efFile2 = $proj.SelectSingleNode("/p:Project/p:ItemGroup/p:Compile[contains(@Include, 'EntityFrameworkModule')]", $xmlNameSpace)
	}
	
	
	$nhFile = $proj.SelectSingleNode("/p:Project/p:ItemGroup/p:None[contains(@Include, 'NHibernateProfilerBootstrapper')]", $xmlNameSpace)
	
	if($nhFile -eq $null){
		$nhFile = $proj.SelectSingleNode("/p:Project/p:ItemGroup/p:Compile[contains(@Include, 'NHibernateProfilerBootstrapper')]", $xmlNameSpace)
	}
	
	$nhFile2 = $proj.SelectSingleNode("/p:Project/p:ItemGroup/p:None[contains(@Include, 'NhibernateModule')]", $xmlNameSpace)
	
	if($nhFile2 -eq $null){
		$nhFile2 = $proj.SelectSingleNode("/p:Project/p:ItemGroup/p:Compile[contains(@Include, 'NhibernateModule')]", $xmlNameSpace)
	}
	
	if($orm -eq "nh"){
		$hintPath.set_InnerText("..\packages\NHibernateProfiler.2.0.2143.0\lib\HibernatingRhinos.Profiler.Appender.dll")
		
		$node = $proj.CreateElement("None", $ns)
		$node.SetAttribute("Include", "App_Start\EntityFrameworkProfilerBootstrapper.cs")
		
		$efFile.ParentNode.AppendChild($node)
		$efFile.ParentNode.RemoveChild($efFile)
		
		$node = $proj.CreateElement("None", $ns)
		$node.SetAttribute("Include", "Backend\EntityFramework\DependencyInjection\EntityFrameworkModule.cs")
		
		$efFile2.ParentNode.AppendChild($node)
		$efFile2.ParentNode.RemoveChild($efFile2)
		
		$node = $proj.CreateElement("Compile", $ns)
		$node.SetAttribute("Include", "App_Start\NHibernateProfilerBootstrapper.cs")
		
		$nhFile.ParentNode.AppendChild($node)
		$nhFile.ParentNode.RemoveChild($nhFile)
		
		$node = $proj.CreateElement("Compile", $ns)
		$node.SetAttribute("Include", "Backend\Nhibernate\DependencyInjection\NhibernateModule.cs")
		
		$nhFile2.ParentNode.AppendChild($node)
		$nhFile2.ParentNode.RemoveChild($nhFile2)
	}
	
	elseif($orm -eq "ef") {
		$hintPath.set_InnerText("..\packages\EntityFrameworkProfiler.2.0.2143.0\lib\HibernatingRhinos.Profiler.Appender.dll")
		
		$node = $proj.CreateElement("None", $ns)
		$node.SetAttribute("Include", "App_Start\NHibernateProfilerBootstrapper.cs")
		
		$nhFile.ParentNode.AppendChild($node)
		$nhFile.ParentNode.RemoveChild($nhFile)
		
		$node = $proj.CreateElement("None", $ns)
		$node.SetAttribute("Include", "Backend\Nhibernate\DependencyInjection\NhibernateModule.cs")
		
		$nhFile2.ParentNode.AppendChild($node)
		$nhFile2.ParentNode.RemoveChild($nhFile2)
		
		$node = $proj.CreateElement("Compile", $ns)
		$node.SetAttribute("Include", "App_Start\EntityFrameworkProfilerBootstrapper.cs")
		
		$efFile.ParentNode.AppendChild($node)
		$efFile.ParentNode.RemoveChild($efFile)
		
		$node = $proj.CreateElement("Compile", $ns)
		$node.SetAttribute("Include", "Backend\EntityFramework\DependencyInjection\EntityFrameworkModule.cs")
		
		$efFile2.ParentNode.AppendChild($node)
		$efFile2.ParentNode.RemoveChild($efFile2)
	}
		
	$proj.Save("$($filename)") | Out-Null
}