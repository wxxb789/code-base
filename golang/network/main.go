package main

import (
	"bufio"
	"encoding/binary"
	"flag"
	"fmt"
	"math/rand"
	"net"
	"os"
	"strconv"
	"time"
)

type Proto struct {
	PackLen   int32
	HeaderLen int16
	Ver       int16
	Operation int16
	Seq       int32
	Body      []byte
}

func main() {
	flag.Parse()
	begin, err := strconv.Atoi(os.Args[1])
	if err != nil {
		panic(err)
	}
	num, err := strconv.Atoi(os.Args[2])
	if err != nil {
		panic(err)
	}
	for i := begin; i < begin+num; i++ {
		go client(int64(i))
	}

	var exit chan bool
	<-exit
}

func client(mid int64) {
	for {
		startClient(mid)
		time.Sleep(time.Duration(rand.Intn(10)) * time.Second)
	}
}

func startClient(key int64) {
	quit := make(chan bool, 1)
	defer func() {
		close(quit)
	}()
	conn, err := net.Dial("tcp", os.Args[3])
	if err != nil {
		fmt.Errorf("net.Dial(%s) error(%v)", os.Args[3], err)
		return
	}
	rd := bufio.NewReader(conn)
	proto := new(Proto)
	for {
		if err = tcpReadProto(rd, proto); err != nil {
			fmt.Errorf("key:%d tcpReadProto() error(%v)", key, err)
			quit <- true
			return
		}
		fmt.Sprintf("key: %d op: %d msg: %s", key, proto.Operation, string(proto.Body))
	}
}

func tcpReadProto(rd *bufio.Reader, proto *Proto) (err error) {
	var (
		packLen   int32
		headerLen int16
	)

	if err = binary.Read(rd, binary.BigEndian, &packLen); err != nil {
		return
	}
	if err = binary.Read(rd, binary.BigEndian, &headerLen); err != nil {
		return
	}
	if err = binary.Read(rd, binary.BigEndian, &proto.Ver); err != nil {
		return
	}
	if err = binary.Read(rd, binary.BigEndian, &proto.Operation); err != nil {
		return
	}
	if err = binary.Read(rd, binary.BigEndian, &proto.Seq); err != nil {
		return
	}
	var (
		n, t    int
		bodyLen = int(packLen - int32(headerLen))
	)
	if bodyLen > 0 {
		proto.Body = make([]byte, bodyLen)
		for {
			if t, err = rd.Read(proto.Body[n:]); err != nil {
				return
			}
			if n += t; n == bodyLen {
				break
			}
		}
	} else {
		proto.Body = nil
	}
	return
}
