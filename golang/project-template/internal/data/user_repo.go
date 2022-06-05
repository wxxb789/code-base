package data

import (
	"gorm.io/gorm"
)

type UserRepository interface {
	QueryUserByid(uid int32) (UserPO, error)
}

type UserRepoImpl struct {
	db gorm.DB
}

func NewUserRepoImpl(ormDB *gorm.DB) *UserRepoImpl {
	return &UserRepoImpl{
		db: *ormDB,
	}
}

func (u *UserRepoImpl) QueryUserByid(uid int32) (UserPO, error) {

	var user UserPO

	u.db.Where("id = ?", uid).First(&user)

	return user, nil

}

type UserPO struct {
	Id     int32
	Name   string
	Gender int32
	Age    int32
}
