//go:build wireinject
// +build wireinject

package main

import (
	"github.com/google/wire"
)

func InitializeService() *service.UserService {

	wire.Build(
		service.NewUserService,
		biz.NewUserBiz,
		wire.Bind(new(data.UserRepository), new(*data.UserRepoImpl)),
		data.NewUserRepoImpl,
		data.NewDB)

	return &service.UserService{}

}
