package main

import (
	"database/sql"
	"fmt"

	"github.com/pkg/errors"
)

func f1() error {
	return errors.Wrap(sql.ErrNoRows, "f1 failed")
}

func f2() error {
	err := foo()
	return errors.Wrap(err, "f2 failed")
}

func main() {
	err := f2()
	if errors.Cause(err) == sql.ErrNoRows {
		fmt.Printf("data not found, %v\n", err)
		fmt.Printf("%+v\n", err)
		return
	}

	if err != nil {
		// unknown error
		return
	}

}
