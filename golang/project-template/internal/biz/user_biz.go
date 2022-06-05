package biz

import (
	"errors"
	"wxxb789/code-snippets/golang/project-template/internal/data"
)

type UserBiz struct {
	userRepo data.UserRepository
}

func (biz *UserBiz) GetUserById(uid int32) (*UserDO, error) {

	if uid == 0 {
		return nil, errors.New("error")
	}

	user, err := biz.userRepo.QueryUserByid(uid)

	if err != nil {
	}

	return &UserDO{
		Id:     user.Id,
		Name:   user.Name,
		Gender: user.Gender,
		Age:    user.Age,
	}, nil

}

type UserDO struct {
	Id     int32
	Name   string
	Gender int32
	Age    int32
}
