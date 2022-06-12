package main

import (
	"container/list"
	"fmt"
	"log"
	"sync"
	"time"
)

const (
	PASS int = 1
	ERR  int = 2
)

type metrics struct {
	pass int
	err  int
}

type slideWindow struct {
	bucket int
	time   int64
	event  map[int64]*metrics
	data   *list.List
	sync.RWMutex
}

func NewSlideWindow(bucket int) *slideWindow {
	sw := &slideWindow{}
	sw.bucket = bucket
	sw.data = list.New()
	return sw
}

func (sw *slideWindow) AddEvent(metric metrics) {
	if metric.pass != 0 {
		sw.add(PASS)
	}
	if metric.err != 0 {
		sw.add(ERR)
	}

}

func (sw *slideWindow) add(t int) {
	sw.Lock()
	defer sw.Unlock()
	nowTime := time.Now().Unix()
	if _, ok := sw.event[nowTime]; !ok {
		sw.event = make(map[int64]*metrics)
		sw.event[nowTime] = &metrics{}
	} else {

	}
	if sw.time == 0 {
		sw.time = nowTime
	}
	if sw.time != nowTime {
		sw.data.PushBack(sw.event[nowTime])
		sw.time = nowTime
		if sw.data.Len() > sw.bucket {
			for i := 0; i <= sw.data.Len()-sw.bucket; i++ {
				sw.data.Remove(sw.data.Front())
			}
		}
	}

	switch t {
	case PASS:
		sw.event[nowTime].pass++
	case ERR:
		sw.event[nowTime].err++
	default:
		log.Fatal("err type")
	}

}

func (sw *slideWindow) GetData() *metrics {
	sw.RLock()
	defer sw.RUnlock()
	var metric = &metrics{}
	for i := sw.data.Front(); i != nil; i = i.Next() {
		v := i.Value.(*metrics)
		metric.pass += v.pass
		metric.err += v.err
	}
	for i := sw.data.Front(); i != nil; i = i.Next() {
		fmt.Print("data：", i.Value)
	}
	return metric
}
