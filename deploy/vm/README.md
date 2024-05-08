# Setup VM

## Mount data disk

## Create volumes for docker compose

```bash
sudo docker volume create --name miniflux-db-data --opt type=none --opt device=/datadrive/data/miniflux-db-data --opt o=bind
sudo docker volume create --name rsshub-redis-data --opt type=none --opt device=/datadrive/data/rsshub-redis-data --opt o=bind
sudo docker volume create --name wewe-rss-app-data --opt type=none --opt device=/datadrive/data/wewe-rss-app-data --opt o=bind
sudo docker volume create --name memos-data --opt type=none --opt device=/datadrive/data/memos-data --opt o=bind
```
