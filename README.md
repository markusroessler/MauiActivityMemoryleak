## .NET Heap Dump
```shell
adb shell setprop debug.mono.profile '127.0.0.1:9000,nosuspend,connect'
adb reverse tcp:9000 tcp:9001  
dotnet-dsrouter android 
dotnet-gcdump collect -p 38604   
```

## Java Heap Dump
```shell
am dumpheap com.companyname.mauiactivitymemoryleak /data/local/tmp/heapdump.hprof   
adb pull /data/local/tmp/heapdump.hprof
```