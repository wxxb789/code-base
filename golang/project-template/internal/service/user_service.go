package service

import "wxxb789/code-snippets/golang/project-template/internal/biz"

type UserDTO struct {
	Name   string `json:"name"`
	Gender int32  `json:"gender"`
	Age    int32  `json:"age"`
}

type UserService struct {
	userBiz *biz.UserBiz
}

func (s *UserService) UserInfo(uid int32) (*UserDTO, error) {

	do, error := s.userBiz.GetUserById(uid)
	if error != nil {
	}
	return &UserDTO{
		Name:   do.Name,
		Gender: do.Gender,
		Age:    do.Age,
	}, nil
}
