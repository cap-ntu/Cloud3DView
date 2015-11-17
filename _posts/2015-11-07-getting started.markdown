---
layout: post
title:  "Getting Started"
categories: unicap
---
#<span style="color: #4499ee">Getting Started</span> 
---

<div  align="center">    
<img src="http://cap-ntu.github.io/UniCAP/img/unicap_start.jpg" width="400" />
</div>

###<span style="color: #43a102">Define the Vertex</span>

    ntu::cap::DAG::create_table (table_name, shard_num, partition_algo)
    ntu::cap::DAG::create_cf<T> (table_name, cf_name, cf_type)
    ntu::cap::DAG::load_from_hdfs (hdfs_path, table_name, cf_name)
    ntu::cap::DAG::load_local_file (local_path, table_name, cf_name)
    
###<span style="color: #43a102">Define the Edge (Input/Output Path)</span>

    ntu::cap::Storage::vector_put<T> 
    ntu::cap::Storage::vector_get<T>
    ntu::cap::Storage::scan<T>
    ntu::cap::Storage::timed_put<T> 
    ntu::cap::Storage::timed_scan<T>
    
###<span style="color: #43a102">Define the Edge (Computing Functions)</span>

    class UCPUFunctions : public CPUFunctions {
    public:
	    static UCPUFunctions& singleton() {
	        static UCPUFunctions u_cpu_function;
	        return u_cpu_function;
	    }
	
	    UCPUFunctions() : CPUFunctions() {
	        CPUFunctions::_cpu_functions_p["User_functions"] = hello_world;
	    }
	
	    static int64_t User_functions (TaskNode new_task) {
	        //User implementation
	        return 1;
	    }
    }