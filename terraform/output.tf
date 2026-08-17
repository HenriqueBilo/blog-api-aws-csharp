output "dynamodb_posts_table" {
  value = aws_dynamodb_table.blog-api-posts.name
}

output "dynamodb_comments_table" {
  value = aws_dynamodb_table.blog-api-comments.name
}

output "s3_bucket_name" {
  value = aws_s3_bucket.bucket_images.bucket
}

output "cognito_user_pool_id" {
  value = aws_cognito_user_pool.cognito_user_pool.id
}

output "cognito_client_id" {
  value = aws_cognito_user_pool_client.cognito_pool_client.id
}

output "sns_topic_arn" {
  value = aws_sns_topic.novo_comentario.arn
}

output "sqs_queue_url" {
  value = aws_sqs_queue.notificacoes.id
}

output "cloudwatch_log_group" {
  value = aws_cloudwatch_log_group.app_logs.name
}