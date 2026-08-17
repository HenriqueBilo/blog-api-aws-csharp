terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 6.0"
    }
	
	random = {
	  source = "hashicorp/random"
	  version = "~> 3.0"
	}
  }
  
  backend "s3" {
    bucket         = "blog-api-terraform-state-ca2b3dda"
    key            = "blog-api/terraform.tfstate"
    region         = "us-east-1"
    dynamodb_table = "terraform-locks"
    encrypt        = true
  }
}

provider "aws" {
  # Configuration options
  region = "us-east-1"
}

provider "random" {

}

resource "aws_dynamodb_table" "blog-api-posts" {
  name             = "blog-api-posts"
  hash_key         = "PostId"
  billing_mode     = "PAY_PER_REQUEST"

  attribute {
    name = "PostId"
    type = "S"
  }
}

resource "aws_dynamodb_table" "blog-api-comments" {
  name             = "blog-api-comments"
  hash_key         = "CommentId"
  billing_mode     = "PAY_PER_REQUEST"

  attribute {
    name = "CommentId"
    type = "S"
  }
  
  attribute {
    name = "PostId"
    type = "S"
  }
  
  global_secondary_index {
	name = "PostId-Index"
	hash_key = "PostId"
	projection_type = "ALL"
  }
}

resource "random_id" "random_sufix_s3" {
  byte_length = 4
}

resource "aws_s3_bucket" "bucket_images" {
  bucket = "blog-api-images-${random_id.random_sufix_s3.hex}"
}

resource "aws_s3_bucket_public_access_block" "bucket_images" {
  bucket = aws_s3_bucket.bucket_images.id

  block_public_acls       = true
  block_public_policy     = true
  ignore_public_acls      = true
  restrict_public_buckets = true
}

resource "aws_cognito_user_pool" "cognito_user_pool" {
  # O banco de usuários em si (emails, senhas com hash, MFA, etc.)

  name = "blog-api-users"
  auto_verified_attributes = ["email"]
  
  password_policy {
	minimum_length = 8
	require_uppercase = true
	require_numbers = true
	require_symbols = false
  }
}

resource "aws_cognito_user_pool_client" "cognito_pool_client" {
  # A chave de identificação da aplicação dentro daquele User Pool. Necessário pois um mesmo user pool pode ser utilizado por aplicações diferentes (mobile e web por ex)

  name         = "blog-api-client"
  user_pool_id = aws_cognito_user_pool.cognito_user_pool.id
  explicit_auth_flows = ["ALLOW_USER_PASSWORD_AUTH", "ALLOW_REFRESH_TOKEN_AUTH"]
  generate_secret = false
}

resource "aws_sns_topic" "novo_comentario" {
  name = "blog-api-novo-comentario"
}

resource "aws_sqs_queue" "notificacoes" {
  name = "blog-api-notificacoes"
}

resource "aws_sqs_queue_policy" "permite_sns" {
  # Política que permite o SNS publicar na fila

  queue_url = aws_sqs_queue.notificacoes.id

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Effect    = "Allow"
      Principal = { Service = "sns.amazonaws.com" }
      Action    = "sqs:SendMessage"
      Resource  = aws_sqs_queue.notificacoes.arn
      Condition = {
        ArnEquals = { "aws:SourceArn" = aws_sns_topic.novo_comentario.arn }
      }
    }]
  })
}

resource "aws_sns_topic_subscription" "sqs_subscription" {
  # A "inscrição" (subscription): conecta o tópico à fila
  # Isso é o que efetivamente diz "toda mensagem publicada nesse tópico, manda também pra essa fila".

  topic_arn = aws_sns_topic.novo_comentario.arn
  protocol  = "sqs"
  endpoint  = aws_sqs_queue.notificacoes.arn
}

resource "aws_cloudwatch_log_group" "app_logs" {
  # É só o "diretório" onde os logs da sua aplicação vão ser guardados no CloudWatch

  name              = "/blog-api/app"
  retention_in_days = 14
}

