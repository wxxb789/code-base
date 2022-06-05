package main

import (
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
)

func main() {

	r := gin.Default()
	r.GET("/user/:uid", func(ctx *gin.Context) {

		uid := ctx.Param("uid")
		i, err := strconv.ParseInt(uid, 10, 32)

		if err != nil {
		}

		user, err := InitializeService().UserInfo(int32(i))

		ctx.JSON(http.StatusOK, user)
	})

	r.Run()
}
