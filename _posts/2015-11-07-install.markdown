---
layout: post
title:  "Install UniCAP"
categories: unicap
---
#Install UniCAP
These are instructions for installing UniCAP.

## Using Docker Image

We provide an Docker image with the stable version of UniCAP installed. Users can deploy UniCAP using our Big Data Platform (BDP) directly with the associated Docker image.

[Using UniCAP in BDP].


##Installing From Source
    
###<span style="color: Green">Requirements</span>

####<span style="color: blue">C++ Compiler</span>

UniCAP needs a C++ compiler supporting c++11, such as gcc >= 4.7.2 (prefer >= 4.8) or llvm >= 3.4. You can update gcc via either downloading packages, or building from source. 

**1) Ubuntu 12.04**

    sudo apt-get install python-software-properties
    sudo add-apt-repository ppa:ubuntu-toolchain-r/test
    sudo apt-get update
    sudo apt-get install gcc-4.8 g++-4.8
    sudo update-alternatives --install /usr/bin/gcc gcc /usr/bin gcc-4.8 50
    sudo update-alternatives --install /usr/bin/g++ g++ /usr/bin g++-4.8 50
    sudo update-alternatives --config gcc
    sudo update-alternatives --config g++
    
After that, check the gcc version via:

    gcc --version
    
**2) Ubuntu 14.04**

Ubuntu 14.04 contains gcc-4.8 in its system software system. 
Check the gcc version:

    gcc --version 

---    
    
####<span style="color: blue">Boost Library</span>
    
UniCAP needs boost library >= 1.54 (prefer >= 1.55). 

**1) Ubuntu 12.04**

You can install boost via ppa in ubuntu 12.04:

Add ppa:boost-latest/ppa to your system's software sources based on the instructions in 

[https://launchpad.net/~boost-latest/+archive/ubuntu/ppa](https://launchpad.net/~boost-latest/+archive/ubuntu/ppa)
 

Update your system's software sources:

    sudo apt-get update
    
Install boost 1.55:

	sudo apt-get install libboost1.55-all-dev
    
 **2) Ubuntu 14.04**   
 
 Ubuntu 14.04 contains boost1.55
 
 	sudo apt-get install libboost1.55-all-dev

---    
    
####<span style="color: blue">Thrift</span> 

1) Install thrift dependencies:

    sudo apt-get install libevent-dev automake libtool
    sudo apt-get install flex bison pkg-config libssl-dev  
      
2) Install thrift from source 

    wget http://mirror.nus.edu.sg/apache/thrift/0.9.2/thrift-0.9.2.tar.gz
    tar zxvf thrift-0.9.2.tar.gz
    cd thrift-0.9.2
    ./configure --without-java --without-go
    make
    sudo make install
    
---    
    
####<span style="color: blue">Glog and Gflags</span> 

UniCAP uses glog and gflags to print log data.

**1) Ubuntu 12.04**

Install Glog

    wget https://google-glog.googlecode.com/files/glog-0.3.3.tar.gz
    tar zxvf glog-0.3.3.tar.gz
    cd glog-0.3.3
    ./configure
    make
    sudo make install
    
Install Gflags

    wget https://github.com/schuhschuh/gflags/archive/master.zip
    unzip master.zip
    cd gflags-master
    mkdir build && cd build
    export CXXFLAGS="-fPIC" && cmake .. && make VERBOSE=1
    make
    sudo make install
    
**2) Ubuntu 14.04**

sudo apt-get install libgflags-dev libgoogle-glog-dev

---

####<span style="color: blue">LevelDB</span> 

UniCAP uses LevelDB as one of its storage systems.

    sudo apt-get install libleveldb-dev libsnappy-dev

---    

####<span style="color: blue">OpenCV2</span> 

UniCAP uses OpenCV2 run image related applications.

    wget https://github.com/Itseez/opencv/archive/2.4.11.zip
    unzip 2.4.11.zip
    ./configure
    make 
    sudo make install
---

####<span style="color: blue">MPI</span> 

UniCAP uses MPI to parallel the application.

    sudo apt-get install libopenmpi-dev openmpi-bin openmpi-common openmpi-doc

---

####<span style="color: blue">HDFS</span> 

To work with Hadoop Distributed File System using C++, there are two choices:

**1) libhdfs3**

Libhdfs3 a native c/c++ hdfs client (there is no need to use Java to work with HDFS in application layer).

Refer to [https://github.com/PivotalRD/libhdfs3](https://github.com/PivotalRD/libhdfs3)

To compile libhdfs3, users need following libraries:
 
    sudo apt-get install protobuf-compiler  libprotobuf-dev 
    sudo apt-get install libkrb5-dev libxml2-dev libuuid1 uuid-dev 
    sudo apt-get install libgsasl7 libgsasl7-dev 


**2) libhdfs**

Libhdfs is a JNI based C api for Hadoop's DFS. It provides a simple subset of C apis to manipulate DFS files and the filesystem. 

Clients should compile libhdfs based on their HDFS version in use. To work with JNI, there are some modifications in system:

    vi /etc/ld.so.conf.d/java.conf
    
    append following configurations:
    
    /opt/jdk1.7.0_75/jre/lib/amd64
    /opt/jdk1.7.0_75/jre/lib/amd64/server

Sometimes, the JNI-based application cannot find the needed jar packets for HDFS operations. Using following commands to generate a shell script, run it at the start up of the application:

    find /usr/lib/hadoop -name *.jar|awk '{ printf("export CLASSPATH=%s:$CLASSPATH\n", $0); }' >> hdfs_jni.sh
    find /usr/lib/hadoop/lib -name *.jar|awk '{ printf("export CLASSPATH=%s:$CLASSPATH\n", $0); }' >> hdfs_jni.sh
    find /usr/lib/hadoop-hdfs/ -name *.jar|awk '{ printf("export CLASSPATH=%s:$CLASSPATH\n", $0); }' >> hdfs_jni.sh
    find /usr/lib/hadoop-hdfs/lib/ -name *.jar|awk '{ printf("export CLASSPATH=%s:$CLASSPATH\n", $0); }' >> hdfs_jni.sh
    
---
---

###<span style="color: Green">Compile UniCAP</span>
    git clone https://github.com/sunpengsdu/unicap
    cd unicap/build
    cmake ..
    make -j8

[Using UniCAP in BDP]: http://155.69.146.43/bdp/guest
